using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using UnityEngine.SceneManagement;
using Zenject;

namespace VKLib
{
    /// <summary>
    ///     //세이브 로드 절차 간단 설명//
    ///     세이브를 시도하면, 우선 세이브를 해야 할 객체들에게 '준비'를 시킨다. (PrepareSave 이벤트로)
    ///     그러면 각 객체들은 자신의 정보를 세이브 할 객체 (DTO들) 에 쓴다.
    ///     <see cref="DTOContainer" /> 를 시리얼 해서, 세이브 슬롯 (헤더)의 이름을 사용하여 저장한다.
    ///     로드를 시도하면, StartSetting 스태틱 클래스에 어떤 파일을 로드할것인지와 로드할지 여부를 설정한다.
    ///     그리고 메인 씬을 다시 읽는다.
    ///     Init 이나 execute의 첫 프레임에 StartSetting.IsloadGame 을 이용하여 분기,
    ///     게임 시작용 객체생성을 할 것인지, 아니면 로드데이터를 이용하여 객체들을 생성할지를 결정, 처리한다.
    ///     (zenject.initialize 시점에는 모든 매니저들이 init이 끝난것을 보증할 수 없기에 <see cref="CreateObjectsForStartSystem" /> 의 init 시점을 쓰는 것)
    ///     (씬로드가 조금 변태적이지만, 메모리적인 측면에서는 깔끔할 수도 있다는 생각도 든다)
    ///     실물 객체들은 _eventManager.Broadcast_Event_ApplyLoadedData() 와 _eventManager.Broadcast_Event_ApplyLoadedDataLate() 에서
    ///     생성됨.
    ///     late 가 있는 이유는, system 들에서 객체를 생성 후 늦은 주입을 해줄 필요가 있기 때문. 저장소를 넣어준다거나)
    /// </summary>
    public class SaveLoader
    {
        /// <summary>
        ///     생성자 주입을 쓰고 있는데, 이는 다른 클래스보다 빠르게 초기화 할 필요가 있기 때문이다.
        ///     시작시 저장폴더와 세이브파일 헤더가 없다면 생성한다.
        /// </summary>
        [Inject]
        public SaveLoader(EventManager eventManager, IDTOContainer DTOContainer, IProjectSetting projectSetting)
        {
            TDebug.AssertNotNull(eventManager, nameof(eventManager));
            _eventManager = eventManager;
            TDebug.AssertNotNull(DTOContainer, nameof(DTOContainer));
            _dtoContainer = DTOContainer;
            TDebug.AssertNotNull(projectSetting, nameof(projectSetting));
            _projectSetting = projectSetting;

            CreateSaveFolderIfNotExist();
            CreateEmptySaveFileHeadersFileIfNotExist();
            _saveFileHeaders = LoadSaveFileHeaders();

            if (projectSetting.IsLoadGame)
            {
                LoadContainer();
            }
        }

        [Inject] private readonly PlayerData _playerData;
        public bool IsLastSaveFileExist => File.Exists(GetSaveFilePath("Save0"));
        private readonly IDTOContainer _dtoContainer;
        private readonly EventManager _eventManager;
        private readonly IProjectSetting _projectSetting;
        private SaveFileHeaders _saveFileHeaders;

        public void CreateSaveFolderIfNotExist()
        {
            if (Directory.Exists(_projectSetting.GetSaveFolderPath()))
            {
                return;
            }

            Directory.CreateDirectory(_projectSetting.GetSaveFolderPath());
        }

        public void DeleteSave(int slotNumber)
        {
            var newHeader = MakeEmptyHeader(slotNumber);

            //세이브파일 헤더들의 정보를 저장한다.
            _saveFileHeaders.SetHeader(newHeader.SlotNumber, newHeader);
            SaveHeaderData(_saveFileHeaders);
        }

        /*
        public async Task AutoSaveGameAsync()
        {
            TDebug.Log($"자동 저장을 시도합니다.");
    
            var autoSaveSlotNumber = _saveFileHeaders.GetOldestAutoSaveSlotNumber();
            var currentHeader = WriteHeaderToAutoSave(autoSaveSlotNumber);
            await SaveDataToFileAsync(currentHeader.FileName);
            _saveFileHeaders.SetHeader(currentHeader.SlotNumber, currentHeader);
            SaveHeaderData(_saveFileHeaders);
        }*/

        public IEnumerable<SaveHeader> FatchSaveHeaders()
        {
            LoadSaveFileHeaders();
            return _saveFileHeaders.Headers;
        }

        public SaveHeader GetSaveFileHeader(string fileName)
        {
            return _saveFileHeaders.Headers.FirstOrDefault(x => x.FileName == fileName);
        }

        public string GetSaveFilePath(string fileName)
        {
            var path = Path.Combine(_projectSetting.GetSaveFolderPath(), fileName) + ".json";
            return path;
        }

        public SaveHeader GetSaveHeaderBySlotNumber(int slotNumber)
        {
            return _saveFileHeaders.GetHeader(slotNumber);
        }

        public string GetSaveHeaderFilePath()
        {
            return Path.Combine(_projectSetting.GetSaveFolderPath(), "SaveFileHeaders.json");
        }

        /// <summary>
        ///     게임을 시작할때 플래그에 따라 로드가 일어남.
        ///     주의! privateSetter 를 디시리얼 하기위해서는 별도의 리졸버가 필요함!
        ///     https://talkdotnet.wordpress.com/2019/03/15/newtonsoft-json-deserializing-objects-that-have-private-setters/
        /// </summary>
        public void LoadContainer()
        {
            var json = File.ReadAllText(GetSaveFilePath(_projectSetting.FileName));
            var newObjectContainer = JsonConvert.DeserializeObject<DTOContainer>(json, new JsonSerializerSettings
            {
                ContractResolver = new PrivateResolver(), ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
            });

            _dtoContainer.UpdateWith(newObjectContainer);
            TDebug.Log("LoadData done!");
        }

        public SaveFileHeaders LoadSaveFileHeaders()
        {
            var json = File.ReadAllText(GetSaveFilePath("SaveFileHeaders"));
            _saveFileHeaders = JsonConvert.DeserializeObject<SaveFileHeaders>(json);
            return _saveFileHeaders;
        }

        /// <summary>
        ///     로드할 정보를 세팅하고 씬을 재 시작한다.
        ///     씬 전환시 데이터를 넘기기 위해 스태틱을 사용하는 것에 주의.
        /// </summary>
        public void PrepareLoadGameAndRestartScene(int slotNumber)
        {
            _projectSetting.IsLoadGame = true;
            _projectSetting.FileName = _saveFileHeaders.GetHeader(slotNumber).FileName;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public async Task SaveDataToFileAsync(string savefileName)
        {
            //데이터를 준비하게 한다.
            _eventManager.Broadcast_Event_PrepareSave();

            var saveFilePath = GetSaveFilePath(savefileName);
            //var backupFilePath = Tlib.GetZipBackupFilePath("lastSaveBackUp");

            _eventManager.Broadcast_Event_SaveDone();
        }

        public async Task SaveGameToSlotAsync(int slotNumber)
        {
            TDebug.Log("저장을 시도합니다.");

            //자동저장은 꺼둠
            /*if (_saveFileHeaders.GetHeader(slotNumber).IsAutoSave)
            {
                TDebug.LogWarning("자동저장 슬롯에는 저장할 수 없습니다");
                return;
            }*/

            //헤더의 정보 갱신.
            var newHeader = MakeThisGameHeader(slotNumber);
            _saveFileHeaders.SetHeader(slotNumber, newHeader);
            SaveHeaderData(_saveFileHeaders);

            //게임 정보를 저장한다.
            //await SaveDataToFileAsync(newHeader.FileName);
            SaveGameData(_dtoContainer, slotNumber);

            TDebug.Log("저장 완료.");
        }

        private void CreateEmptySaveFileHeadersFileIfNotExist()
        {
            if (File.Exists(GetSaveHeaderFilePath()))
            {
                return;
            }

            _saveFileHeaders = SaveFileHeaders.GetDefaultHeaders();
            SaveHeaderData(_saveFileHeaders);
        }

        private SaveHeader MakeEmptyHeader(int slotNumber)
        {
            var header = new SaveHeader
            {
                IsEmptySlot = true, SlotNumber = slotNumber, FileName = $"Save{slotNumber}"
            };

            return header;
        }

        private SaveHeader MakeThisGameHeader(int slot)
        {
            var header = new SaveHeader();
            return header;
        }

        private void SaveGameData(IDTOContainer container, int slotNumber)
        {
            try
            {
                //데이터를 준비하게 한다.
                _eventManager.Broadcast_Event_PrepareSave();

                TDebug.Log("...parepare json...");

                //hack : 게임세팅은 딱히 넣어주기 애매하니 여기서 처리. 옮길 수 있으면 옮깁시다.
                //container.GameSettingDTO.PDCGameSetting = _projectSetting;
                var json = JsonConvert.SerializeObject(container);
                TDebug.Log("...json make done...");
                var path = GetSaveFilePath($"{_saveFileHeaders.Headers[slotNumber].FileName}");
                TDebug.Log($"path : {path} ...");
                File.WriteAllText(path, json);

                _eventManager.Broadcast_Event_SaveDone();

                TDebug.Log("SaveData done!");
            }
            catch (Exception e)
            {
                TDebug.Log($"SaveGameDataError! {e}");
                throw;
            }
        }

        private void SaveHeaderData(SaveFileHeaders headers)
        {
            var json = JsonConvert.SerializeObject(headers, Formatting.Indented);
            var path = GetSaveFilePath("SaveFileHeaders");

            File.WriteAllText(path, json);
            TDebug.Log("SaveEntitiesInScene done!");
        }

        private SaveHeader WriteHeaderToAutoSave(int slotNumber)
        {
            var header = MakeThisGameHeader(slotNumber);
            header.IsAutoSave = true;
            return header;
        }
    }

    /// <summary>
    ///     프로젝트별로 공통되는 세팅
    /// </summary>
    public interface IProjectSetting
    {
        string FileName { get; set; }
        bool IsLoadGame { get; set; }
        string GetSaveFolderPath();
    }

    public interface IDTOContainer
    {
        void UpdateWith(DTOContainer newObjectContainer);
    }

    /// <summary>
    ///     privateSetter를 해결할 수 있는 Json.net 리졸버
    /// </summary>
    public class PrivateResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var prop = base.CreateProperty(member, memberSerialization);
            if (!prop.Writable)
            {
                var property = member as PropertyInfo;
                var hasPrivateSetter = property?.GetSetMethod(true) != null;
                prop.Writable = hasPrivateSetter;
            }

            return prop;
        }
    }

    /// <summary>
    ///     bug : GameSetting을 별도로 세팅? 해주지도 않는데 세이브 로드에서 게임세팅 값이 잘 세이브 로드 된다... 아리송송.
    ///     bug : 적어도 UpdateWith 에서 일어나는 건 아님.
    ///     주의. DTO들은 'private setter'를 가지지 않는 편이 좋음. 디시리얼 할때 누락될 가능성이 있다. 찾기 어려운 버그를 낳음.
    /// </summary>
    public class DTOContainer
    {
        //public PlayerDataDTO PlayerDataDTO;
        /*//dto들
        public ActorManagerDTO ActorManagerDTO { get; set; }
        public BuildingManagerDTO BuildingManagerDTO { get; set; }
        public GameSettingSaveData GameSettingDTO { get; set; }
        public MapQuery.MapDTO MapDTO { get; set; }
        public PlayerDataDTO PlayerDataDTO { get; set; }

        //참조저장을 위한 리스트들
        public List<Actor> ActorList { get; set; }
        private readonly Dictionary<Type, object> _typeDicMapper = new Dictionary<Type, object>();

        public DTOContainer()
        {
            ActorList = new List<Actor>();
            ActorManagerDTO = new ActorManagerDTO();
            BuildingManagerDTO = new BuildingManagerDTO();
            GameSettingDTO = new GameSettingSaveData();
            MapDTO = new MapQuery.MapDTO();
            PlayerDataDTO = new PlayerDataDTO();

            _typeDicMapper.Add(typeof(Actor), ActorList);
        }

        public T GetObj<T>(int id) where T : ISaveableObj
        {
            var list = _typeDicMapper[typeof(T)] as List<T>;
            return list[id];
        }

        public void UpdateWith(DTOContainer other)
        {
            ActorList = other.ActorList;
            ActorManagerDTO = other.ActorManagerDTO;
            BuildingManagerDTO = other.BuildingManagerDTO;
            GameSettingDTO = other.GameSettingDTO;
            MapDTO = other.MapDTO;
            PlayerDataDTO = other.PlayerDataDTO;
        }

        public T Register<T>(T obj) where T : ISaveableObj
        {
            var list = _typeDicMapper[typeof(T)] as List<T>;
            obj.ID = list.Count;
            list.Add(obj);

            return obj;
        }*/
        public void UpdateWith(DTOContainer newObjectContainer)
        {
            throw new NotImplementedException();
        }

        public T Register<T>(T saveableObj) where T : ISaveableObj
        {
            throw new NotImplementedException();
        }
    }

    public class GameSettingSaveData
    {
        // public PDCGameSetting PDCGameSetting { get; set; }
    }

    public interface ISaveableObj
    {
        int ID { get; set; }
        Type SavingType { get; set; }
        void InitAfterLoad();
    }

    public class SaveableObjFactory<T> : Factory<T> where T : ISaveableObj
    {
        public T CreateAndResist(DTOContainer container)
        {
            var obj = Create();
            return container.Register(obj);
        }
    }
}