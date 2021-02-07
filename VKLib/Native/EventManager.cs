using System;
using System.Runtime.CompilerServices;
using SoundManager;
using UnityEngine;

namespace VKLib.Native
{
    /// <summary>
    ///     특정 게임에 관련하지 않는  라이브러리 공통 이벤트들.
    /// </summary>
    public class EventManager
    {
        #region Debug

        /// <summary>
        ///     호출한 함수를 받아내서 로깅한다.
        ///     인자는 널일 수도 있으니 주의할 것. 파일명을 남길수도 있지만 로그가 쓸데없이 길어져서 일단 무시.
        ///     https://stackoverflow.com/questions/171970/how-can-i-find-the-method-that-called-the-current-method/37885619
        /// </summary>
        private void LoggingEventCall(object[] args, [CallerMemberName] string memberName = ""
                                    , [CallerFilePath] string fileName = "", [CallerLineNumber] int lineNumber = 0)
        {
            var arg = string.Join(",", args);
            //TDebug.Log($"{fileName}, {lineNumber}, {memberName}, args: {arg}");
            TDebug.Log($"{memberName}, args: {arg}");
        }

        public event Func<string, string> ConsoleTextInput;

        public string Raise_ConsoleTextInput(string str)
        {
            return ConsoleTextInput?.Invoke(str);
        }

        public event Action ToggleDebugConsole;

        public virtual void Raise_ToggleDebugConsole()
        {
            ToggleDebugConsole?.Invoke();
        }

        /// <summary>
        ///     VkglDebug
        /// </summary>
        public event Action<Vector3, Vector3, Color> DrawDebugLine;

        public virtual void Raise_DrawDebugLine(Vector3 start, Vector3 end, Color color)
        {
            DrawDebugLine?.Invoke(start, end, color);
        }

        /// <summary>
        ///     VkglDebug
        /// </summary>
        public event Action<Vector3, Vector3, Color> DrawDebugArrow;

        public virtual void Raise_DrawDebugArrow(Vector3 start, Vector3 end, Color color)
        {
            DrawDebugArrow?.Invoke(start, end, color);
        }

        /// <summary>
        ///     VkglDebug
        /// </summary>
        public event Action<Vector3, float, int, Color> DrawDebugRectangle;

        public virtual void Raise_DrawDebugRectangle(Vector3 center, float radious, int points, Color color)
        {
            DrawDebugRectangle?.Invoke(center, radious, points, color);
        }

        #endregion Debug

        #region SystemSetting - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public event Action WriteLogForBackupSaveData;

        /// <summary>
        /// 유저의 세이브데이터 복구를 위해서 현재의 세이브 데이터를 로그의 형태로 서버에 저장한다. 속도를 위해 유효성 검증은 하지 않음.
        /// </summary>
        public void Notify_WriteLogForBackupSaveData()
        {
            WriteLogForBackupSaveData?.Invoke();
        }


        public event Action<float> SystemUIScaleChanged;

        public virtual void Raise_SystemUIScaleChanged(float newScale)
        {
            LoggingEventCall(new object[] {newScale});
            SystemUIScaleChanged?.Invoke(newScale);
        }

        public event Action<int> AutoSaveTermChanged;

        public virtual void Raise_AutoSaveTermChanged(int newTermDay)
        {
            LoggingEventCall(new object[] {newTermDay});
            AutoSaveTermChanged?.Invoke(newTermDay);
        }

        public event Action<string> PlayerVillageNameChanged;

        public virtual void Raise_PlayerVillageNameChanged(string newName)
        {
            LoggingEventCall(new object[] {newName});
            PlayerVillageNameChanged?.Invoke(newName);
        }

        public event Action<string> PlayerTribeNameChanged;

        public virtual void Raise_PlayerTribeNameChanged(string newName)
        {
            LoggingEventCall(new object[] {newName});
            PlayerTribeNameChanged?.Invoke(newName);
        }

        public event Action<int> IncreasePlayerMoney;

        public void Raise_IncreasePlayerMoney(int amount)
        {
            TDebug.Assert(amount > 0);
            IncreasePlayerMoney?.Invoke(amount);
        }

        public event Action<int> DecreasePlayerMoney;

        public void Raise_DecreasePlayerMoney(int amount)
        {
            TDebug.Assert(amount > 0);
            DecreasePlayerMoney?.Invoke(amount);
        }

        public event Action<int> IncreasePlayerContribution;

        public void Raise_IncreasePlayerContribution(int amount)
        {
            TDebug.Assert(amount > 0);
            IncreasePlayerContribution?.Invoke(amount);
        }

        #endregion SystemSetting - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        #region Objective & Achievement- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public event Action<string, int> GiveObjectiveCompleteReward;

        public virtual void Raise_GiveObjectiveCompleteReward(string name, int amount)
        {
            LoggingEventCall(new object[] {name, amount});
            GiveObjectiveCompleteReward?.Invoke(name, amount);
        }

        #endregion Objective & Achievement- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        #region GameLifeCycle - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        //유저가 글자를 입력하는 이벤트. 게임의 많은 조작을 막는다.
        public event Action UserInputTextStart;

        public virtual void Raise_UserInputTextStart()
        {
            LoggingEventCall(new object[] { });
            UserInputTextStart?.Invoke();
        }

        public event Action UserInputTextEnd;

        public virtual void Raise_UserInputTextEnd()
        {
            LoggingEventCall(new object[] { });
            UserInputTextEnd?.Invoke();
        }

        /// <summary>
        ///     주의. 이 이벤트는 GameSpeedSystem 만 리스닝 할 것. 컨티뉴의 애매함이 있기 때문에, GameSpeedSystem 외에는 GameSpeedChanged를 리스닝 할 것!
        /// </summary>
        public event Action PauseGame;

        public virtual void Raise_PauseGame()
        {
            LoggingEventCall(new object[] { });
            PauseGame?.Invoke();
        }

        /// <summary>
        ///     주의. 이 이벤트는 GameSpeedSystem 만 리스닝 할 것. 컨티뉴의 애매함이 있기 때문에, GameSpeedSystem 외에는 GameSpeedChanged를 리스닝 할 것!
        /// </summary>
        public event Action ContinueGame;

        public virtual void Raise_ContinueGame()
        {
            LoggingEventCall(new object[] { });
            ContinueGame?.Invoke();
        }

        /// <summary>
        ///     유니티를 쓰지 않는 클래스들이 게임 시작시 JobStart()를 호출할 수 없기 때문에,
        ///     이 이벤트를 리스닝 하는 것으로 우회구현 할 수 있다.  (리스닝은 IInitializable 나 생성자에서 해야 할 것)
        /// </summary>
        public event Action GameStart;

        public virtual void Raise_GameStart()
        {
            LoggingEventCall(new object[] { });
            GameStart?.Invoke();
        }

        /// <summary>
        ///     씬이 로딩될 것이므로, 즉시 정리작업을 할 것. (딜레이 없이)
        /// </summary>
        public event Action PrepareSceneLoad;

        public virtual void Raise_PrepareSceneLoad()
        {
            LoggingEventCall(new object[] { });
            PrepareSceneLoad?.Invoke();
        }

        /// <summary>
        ///     해당 이름의 씬을 로딩할 것.
        /// </summary>
        public event Action<string> LoadScene;

        public virtual void Raise_LoadScene(string sceneName)
        {
            LoggingEventCall(new object[] {sceneName});
            LoadScene?.Invoke(sceneName);
        }

        //gameSpeed
        public event Action<float> GameSpeedChange;

        public virtual void Raise_GameSpeedChange(float newGameSpeed)
        {
            LoggingEventCall(new object[] {newGameSpeed});
            GameSpeedChange?.Invoke(newGameSpeed);
        }

        /// <summary>
        ///     이벤트로 쏴서 리스닝 할 수 있게 한다. 이 이벤트를 리스닝 하는 것으로 Tick 처리가 가능해진다. 네이티브 용.
        /// </summary>
        public event Action<float> Tick;

        public virtual void Raise_Tick(float deltaSecond)
        {
            Tick?.Invoke(deltaSecond);
        }

        /// <summary>
        ///     업데이트를 하는 게임 오브젝트들의 틱 처리. 업데이트 대용임.
        /// </summary>
        public event Action<float> GoTick;

        public virtual void Raise_GoTick(float deltaSecond)
        {
            GoTick?.Invoke(deltaSecond);
        }

        /// <summary>
        ///     유니티를 쓰지 않는 클래스들이 게임 시작시 Start()를 호출할 수 없기 때문에,
        ///     이 이벤트를 리스닝 하는 것으로 우회구현 할 수 있다.  (리스닝은 IInitializable 나 생성자에서 해야 할 것)
        /// </summary>
        public event Action Event_GameStart;

        public virtual void Broadcast_Event_GameStart()
        {
            LoggingEventCall(new object[] { });
            Event_GameStart?.Invoke();
        }

        /// <summary>
        ///     씬이 로딩될 것이므로, 즉시 정리작업을 할 것. (딜레이 없이)
        /// </summary>
        public event Action Event_PrepareSceneLoad;

        public virtual void Broadcast_Event_PrepareSceneLoad()
        {
            LoggingEventCall(new object[] { });
            Event_PrepareSceneLoad?.Invoke();
        }

        /// <summary>
        ///     해당 이름의 씬을 로딩할 것.
        /// </summary>
        public event Action<string> Event_LoadScene;

        public virtual void Broadcast_Event_LoadScene(string sceneName)
        {
            LoggingEventCall(new object[] {sceneName});
            Event_LoadScene?.Invoke(sceneName);
        }

        #endregion GameLifeCycle - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        #region SaveLoad - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public event Action<int> EventSaveSlotSelected;

        public virtual void Raise_SaveSlotSelected(int slotNumber)
        {
            LoggingEventCall(new object[] {slotNumber});
            EventSaveSlotSelected?.Invoke(slotNumber);
        }

        /// <summary>
        ///     저장이 성공적으로 종료됨을 방송
        /// </summary>
        public event Action SaveDone;

        public virtual void Raise_SaveDone()
        {
            LoggingEventCall(new object[] { });
            SaveDone?.Invoke();
        }

        //save load
        public event Action<int> SaveGameToSlot;

        public virtual void Raise_SaveGameToSlot(int slotNumber)
        {
            LoggingEventCall(new object[] {slotNumber});
            SaveGameToSlot?.Invoke(slotNumber);
        }

        

        public event Action<int> LoadGameToSlot;

        public virtual void Raise_LoadGameToSlot(int slotNumber)
        {
            LoggingEventCall(new object[] {slotNumber});
            LoadGameToSlot?.Invoke(slotNumber);
        }

        public event Action SaveGame;

        public void Notify_SaveGame()
        {
            SaveGame?.Invoke();
        }

        public event Action LoadGame;

        public void Notify_LoadGame()
        {
            LoadGame?.Invoke();
        }

        
        /// <summary>
        ///     데이터 로드가 끝나서 객체 생성이 끝난 상태.
        /// </summary>
        public event Action StartGamePrepareDone;

        public virtual void Raise_StartGamePrepareDone()
        {
            LoggingEventCall(new object[] { });
            StartGamePrepareDone?.Invoke();
        }

        //deleteSavefile
        public event Action<int> DeleteSavedGame;

        public virtual void Raise_DeleteSavedGame(int slotNumber)
        {
            LoggingEventCall(new object[] {slotNumber});
            DeleteSavedGame?.Invoke(slotNumber);
        }

        #endregion SaveLoad - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        //액터들은 아직 쓰지 않지만, 설계 자체가 의미가 있기 때문에 주석으로 살려둠.
        /*#region Actor  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    public event Func<string, Coord, Actor> SpawnActor;

    public Actor Raise_SpawnActor(string actorType, Coord coord)
    {
        return SpawnActor?.Invoke(actorType, coord);
    }
    
    public event Action<Actor> DeSpawnActor;

    public virtual void Raise_DeSpawnActor(Actor actor)
    {
        DeSpawnActor?.Invoke(actor);
    }
    
    public event Func<Actor, Character> SpawnCharacter;

    /// <summary>
    ///     이 함수의 호출권한은 actor 전유
    /// </summary>
    public virtual Character Raise_SpawnCharacter(Actor actor)
    {
        return SpawnCharacter?.Invoke(actor);
    }

    public event Action<Actor> DeSpawnCharacter;

    /// <summary>
    ///     이 함수의 호출권한은 actor 전유
    /// </summary>
    public virtual void Raise_DeSpawnCharacter(Actor actor)
    {
        DeSpawnCharacter?.Invoke(actor);
    }
    
    #endregion Actor  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*/

        #region Notify - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /*
    public event Action<NotifyInfo> ShowNewNotify;

    public void Raise_ShowNewNotify(NotifyInfo notifyEntity)
    {
        ShowNewNotify?.Invoke(notifyEntity);
    }

    public event Action<NotifyInfo> MakeNewNotify;

    public void Raise_MakeNewNotify(NotifyInfo info)
    {
        MakeNewNotify?.Invoke(info);
    }*/

        #endregion Notify - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        #region input  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        ///     유저의 특정 지점을 좌클릭 하였을 때.
        /// </summary>
        public event Action<TVector3> MouseLeftSingleClicked;

        public virtual void Raise_MouseLeftSingleClicked(TVector3 position)
        {
            MouseLeftSingleClicked?.Invoke(position);
        }

        /// <summary>
        ///     유저의 특정 지점을 우클릭 하였을 때.
        /// </summary>
        public event Action<TVector3> MouseRightSingleClicked;

        public virtual void Raise_MouseRightSingleClicked(TVector3 position)
        {
            MouseRightSingleClicked?.Invoke(position);
        }

        /// <summary>
        ///     유저의 특정 지점을 더블 클릭 하였을 때.
        /// </summary>
        public event Action<TVector3> MouseDoubleClicked;

        public virtual void Raise_MouseDoubleClicked(TVector3 position)
        {
            MouseDoubleClicked?.Invoke(position);
        }

        /// <summary>
        ///     유저가 특정 지점을 드래그 하였을 때
        /// </summary>
        public event Action<TVector2Bound> MouseDragged;

        public virtual void Raise_MouseDragged(TVector2Bound bound)
        {
            MouseDragged?.Invoke(bound);
        }

        /// <summary>
        ///     좌클릭 누르고 있을 때.
        ///     UI위에서는 받아들이지 않는다.
        /// </summary>
        public event Action<Coord> PlayerLeftClickAndHoldTile;

        public virtual void Raise_PlayerLeftClickAndHoldTile(Coord coord)
        {
            PlayerLeftClickAndHoldTile?.Invoke(coord);
        }

        /// <summary>
        ///     시프트와 함께 좌클릭 누르기 시작함. 범위 지징 시작 좌표 검출에 쓰인다.
        /// </summary>
        public event Action<Vector3> PlayerLeftClickWithShiftStart;

        public virtual void Raise_PlayerLeftClickWithShiftStart(Vector3 pos)
        {
            PlayerLeftClickWithShiftStart?.Invoke(pos);
        }

        // <summary>
        /// 시프트와 함께 좌클릭 누르기 끝냄. 범위 지징 종료 좌표 검출에 쓰인다.
        /// </summary>
        public event Action<Vector3> PlayerLeftClickWithShiftEnd;

        public virtual void Raise_PlayerLeftClickWithShiftEnd(Vector3 pos)
        {
            PlayerLeftClickWithShiftEnd?.Invoke(pos);
        }

        public event Action<Coord> PlayerRightClickAndHold;

        public virtual void Raise_PlayerRightClickAndHold(Coord coord)
        {
            PlayerRightClickAndHold?.Invoke(coord);
        }

        public event Action<Coord> PlayerRightClickAndHoldWithShift;

        public virtual void Raise_PlayerRightClickAndHoldWithShift(Coord coord)
        {
            PlayerRightClickAndHoldWithShift?.Invoke(coord);
        }

        /// <summary>
        ///     좌클릭 을 뗄 때. 주로 UI 조작이나 버튼 누르기 등에 쓰임.
        ///     UI위에서는 받아들이지 않는다.
        /// </summary>
        public event Action<Coord> PlayerLeftClickUpTile;

        public virtual void Raise_PlayerLeftClickUpTile(Coord coord)
        {
            PlayerLeftClickUpTile?.Invoke(coord);
        }

        // UI위에서는 받아들이지 않는다.
        public event Action<Coord> PlayerRightClickUpTile;

        public virtual void Raise_PlayerRightClickUpTile(Coord coord)
        {
            PlayerRightClickUpTile?.Invoke(coord);
        }

        /// <summary>
        ///     유저가 빈땅을 선택한 경우
        /// </summary>
        public event Action UserSelectedNone;

        public virtual void Raise_UserSelectedNone()
        {
            LoggingEventCall(new object[] { });
            UserSelectedNone?.Invoke();
        }

        /// <summary>
        ///     도움말을 켜고 끈다. 켜고 끈 정보는 GameHelperSetting에 저장되므로, 그걸 받아와서 처리하자.
        /// </summary>
        public event Action<bool> ToggleEnableGameHelper;

        public virtual void Raise_ToggleEnableGameHelper(bool IsEnable)
        {
            ToggleEnableGameHelper?.Invoke(IsEnable);
        }

        /// <summary>
        ///     도움말을 초기화.
        /// </summary>
        public event Action ResetGameHelper;

        public void Raise_ResetGameHelper()
        {
            ResetGameHelper?.Invoke();
        }

        public event Action CloseMainUIs;

        public void Raise_CloseMainUIs()
        {
            CloseMainUIs?.Invoke();
        }

        #endregion input  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        #region Notify - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        ///     게임의 오브젝트 위에 위로 올라가는 숫자를 출력
        /// </summary>
        public event Action<string, Vector3, Color, float> ShowFloatingText_Vector3;

        public void Raise_ShowFloatingText_Vector3(string text, Vector3 worldPosition, Color color, float ySpeed = 10f)
        {
            ShowFloatingText_Vector3?.Invoke(text, worldPosition, color, ySpeed);
        }


        /*/// <summary>
    ///     매우 중대한 경우에 출력하는 노티파티 메시지. 화면을 가림.
    /// </summary>
    public event Action<AlertMessageInfo> AlertMessageSpawn;*/

        //호환성 오버로드
        /*public void Raise_AlertMessageSpawn(string message, List<AlertButtonInfo> buttonOrNull = null, bool isPauseGame = true , PortraitInfo portrait = null, Coord? focusCoord = null)
    {
        var info = new AlertMessageInfo(message, portrait, buttonOrNull, isPauseGame, focusCoord);
        AlertMessageSpawn?.Invoke(info);
    }

    public void Raise_AlertMessageSpawn(AlertMessageInfo info)
    {
        AlertMessageSpawn?.Invoke(info);
    }*/

        /// <summary>
        ///     매우 중대한 입력이 필요할때 출력하는 노티파티 메시지. 화면을 가리고 게임을 멈추고 인풋창을 띄운다.
        /// </summary>
        public event Action<string, Action<string>> InputMessagePopup;

        public void Raise_InputMessagePopup(string message, Action<string> inputConfirmCallback)
        {
            InputMessagePopup?.Invoke(message, inputConfirmCallback);
        }

        #endregion Notify - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        #region LineDebug - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        #endregion LineDebug - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        #region Camera - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public event Action<Transform> CameraFollowTarget;

        public void Raise_CameraFllowTarget(Transform transform)
        {
            CameraFollowTarget?.Invoke(transform);
        }

        public event Action<Coord> CameraMoveTo;

        public void Raise_CameraMoveTo(Coord coord)
        {
            CameraMoveTo?.Invoke(coord);
        }

        public event Action CameraFree;

        public void Raise_CameraFree()
        {
            CameraFree?.Invoke();
        }

        #endregion Camera - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        #region Sound - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public event Func<string, Coord, float, float, float, SmSound> PlaySound;

        public SmSound Raise_PlaySound(string soundName, Coord position, float percent = 1, float pitch = 1, float volume = 1)
        {
            return PlaySound?.Invoke(soundName, position, percent, pitch, volume);
        }

        public event Func<string, float, float, float, SmSound> PlaySoundUI;

        public SmSound Raise_PlaySoundUI(string soundName, float percent = 1, float pitch = 1, float volume = 1)
        {
            return PlaySoundUI?.Invoke(soundName, percent, pitch, volume);
        }

        public event Action<float> AmbientVolumeChanged;

        public void Raise_AmbientVolumeChanged(float volume)
        {
            AmbientVolumeChanged?.Invoke(volume);
        }

        public event Action<float> MusicVolumeChanged;

        public void Raise_MusicVolumeChanged(float volume)
        {
            MusicVolumeChanged?.Invoke(volume);
        }

        public event Action<float> EffectVolumeChanged;

        public void Raise_EffectVolumeChanged(float volume)
        {
            EffectVolumeChanged?.Invoke(volume);
        }

        #endregion Sound - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        #region PostProcess - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        ///     포스트 프로세스의 색온도 조정. 양수는 붉어지고 음수는 파래진다.
        ///     -100~100
        /// </summary>
        public event Action<float> SetPP_Temperature;

        public void Raise_SetPP_Temperature(float temperature)
        {
            SetPP_Temperature?.Invoke(temperature);
        }

        /// <summary>
        ///     포스트 프로세스의 노출 조정. 낮밤의 구분등에 쓰인다. 양수는 밝아지고 음수는 어두워짐.
        ///     -100~100
        /// </summary>
        public event Action<float> ChangePPExposure;

        public void Raise_ChangePPExposure(float exposure)
        {
            ChangePPExposure?.Invoke(exposure);
        }

        #endregion PostProcess - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        #region BugAndAnalyticsReport- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /*
    public event Action<TException> ReportBug;

    public void Raise_ReportBug(TException tException)
    {
        ReportBug?.Invoke(tException);
    }*/

        public event Action<string, string, object> SendUnityAnalyticsCustomEvent;

        public void Raise_SendUnityAnalyticsCustomEvent(string eventName, string key, object value)
        {
            SendUnityAnalyticsCustomEvent?.Invoke(eventName, key, value);
        }

        #endregion BugAndAnalyticsReport- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        #region TileMap - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public event Action ToggleFogOfWar;

        public void Raise_ToggleFogOfWar()
        {
            ToggleFogOfWar?.Invoke();
        }

        /*
    public event Action<string, PDCMapLayer, Coord> SetTileGraphic;

    public void Raise_SetTileGraphic(string timeName, PDCMapLayer layer, Coord coord)
    {
        SetTileGraphic?.Invoke(timeName, layer, coord);
    }

    public event Action<PDCMapLayer, Coord, TColor> SetTileColor;

    public void Raise_SetTileColor(PDCMapLayer layer, Coord coord, TColor color)
    {
        SetTileColor?.Invoke(layer, coord, color);
    }*/

        public event Action<Coord, string> SpawnTileBlock;

        public void Raise_SpawnTileBlock(Coord coord, string itemName)
        {
            SpawnTileBlock?.Invoke(coord, itemName);
        }

        #endregion TileMap - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        #region SaveLoad - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        ///     저장이 성공적으로 종료됨을 방송
        /// </summary>
        public event Action event_SaveDone;

        public virtual void Broadcast_Event_SaveDone()
        {
            LoggingEventCall(new object[] { });
            event_SaveDone?.Invoke();
        }

        //save load
        public event Action<int> Event_SaveGame;

        public virtual void Broadcast_Event_SaveGame(int slotNumber)
        {
            LoggingEventCall(new object[] {slotNumber});
            Event_SaveGame?.Invoke(slotNumber);
        }

        public event Action OpenSaveFileInspector;

        public virtual void Raise_OpenSaveFileInspector()
        {
            OpenSaveFileInspector?.Invoke();
        }

        public event Action Event_ApplyLoadedData;

        /// <summary>
        ///     로드가 끝났으므로, 읽어온 데이터를 각각 적용하도록 한다.
        ///     이것은 mananger 나 system 들이 돌리는 것으로, 액터나 빌딩등의 객체들을 생성하고 값을 세팅하는 단계다.
        /// </summary>
        public virtual void Broadcast_Event_ApplyLoadedData()
        {
            Event_ApplyLoadedData?.Invoke();
        }

        public event Action Event_ApplyLoadedDataLate;

        /// <summary>
        ///     로드가 끝났으므로, 읽어온 데이터를 각각 적용하도록 한다.
        ///     이것은 Actor나 building 등의 객체들이 돌리는 것으로, 반드시 Broadcast_Event_ApplyLoadedData 후에 적용되어야 한다.
        ///     hack : 좋은 설계가 아니다. 좀 더 깔끔한 설계, 혹은 순서도를 확실히 그려두도록 할 것.
        /// </summary>
        public virtual void Broadcast_Event_ApplyLoadedDataLate()
        {
            Event_ApplyLoadedDataLate?.Invoke();
        }

        public event Action Event_PrepareSave;

        /// <summary>
        ///     각 객체들에게 저장에 앞서 데이터를 정리하여 SaveObject 등에 등록하라고 전한다.
        /// </summary>
        public virtual void Broadcast_Event_PrepareSave()
        {
            Event_PrepareSave?.Invoke();
        }

        public event Action<int> Event_LoadGame;

        public virtual void Broadcast_Event_LoadGame(int slotNumber)
        {
            LoggingEventCall(new object[] {slotNumber});
            Event_LoadGame?.Invoke(slotNumber);
        }

        /// <summary>
        ///     데이터 로드가 끝나서 객체 생성이 끝난 상태.
        /// </summary>
        public event Action Event_StartGamePrepareDone;

        public virtual void Broadcast_Event_StartGamePrepareDone()
        {
            LoggingEventCall(new object[] { });
            Event_StartGamePrepareDone?.Invoke();
        }

        //deleteSavefile
        public event Action<int> Event_DeleteSavedGame;

        public virtual void Broadcast_Event_DeleteSavedGame(int slotNumber)
        {
            LoggingEventCall(new object[] {slotNumber});
            Event_DeleteSavedGame?.Invoke(slotNumber);
        }

        #endregion SaveLoad - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        #region AD - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        ///     광고 호출. 게임 리플래시 외의 방법으로 호출할 필요가 있을때 사용할 것.
        /// </summary>
        public event Action<Action, Action, Action> ShowAd;

        public virtual void Raise_ShowAd(Action adFinished, Action adSkipped, Action adFailed)
        {
            ShowAd?.Invoke(adFinished, adSkipped, adFailed);
        }

        /// <summary>
        ///     광고가 나올 상황이면 광고를 띄우고, 광고가 끝나면 씬로드를 한다.
        /// </summary>
        public event Action SceneLoadAfterAdShow;

        public void Raise_SceneLoadAfterAdShow()
        {
            SceneLoadAfterAdShow?.Invoke();
        }

        #endregion
    }
}