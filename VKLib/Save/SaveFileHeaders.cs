using System;
using System.Collections.Generic;
using System.Linq;
using VKLib.Native;

/// <summary>
    /// hack : 좀 어거지지만, 세이브파일은 일단 10개의 슬롯만 가짐. 마지막 3개는 오토세이브 슬롯이고.
    /// </summary>
    [Serializable]
    public class SaveFileHeaders
    {
        private readonly int MaxSlotCount = 10;
        private readonly int AutoSaveSlotCount = 3;
        public readonly List<SaveHeader> Headers = new List<SaveHeader>();

        //empty constructor for Json.Net
        public SaveFileHeaders()
        {
        }

        /// <summary>
        /// 저장된 헤더가 없을때 팩토리로 생성된 헤더.
        /// </summary>
        public static SaveFileHeaders GetDefaultHeaders()
        {
            var headers = new SaveFileHeaders();
            headers.SetToAllEmpty();
            return headers;
        }

        private void SetToAllEmpty()
        {
            for (int i = 0; i < MaxSlotCount; i++)
            {
                if (i < MaxSlotCount - AutoSaveSlotCount)
                {
                    Headers.Add(SaveHeader.MakeEmptyHeader(i, $"save{i}", isAutoSave: false));
                }
                else
                {
                    Headers.Add(SaveHeader.MakeEmptyHeader(i, $"save{i}", isAutoSave: true));
                }
            }
        }

        public SaveHeader GetHeader(int slotNumber)
        {
            return Headers.FirstOrDefault(x => x.SlotNumber == slotNumber);
        }

        public void SetHeader(int slotNumber, SaveHeader newHeader)
        {
            Headers[slotNumber] = newHeader;
        }

        public int GetOldestAutoSaveSlotNumber()
        {
            TDebug.Assert(Headers.Any());

            var oldestAutoSave = Headers
                                 .Where(x => x.IsAutoSave)
                                 .OrderBy(x => x.SaveGameDateTime)
                                 .First();
            return oldestAutoSave.SlotNumber;
        }
    }

    
    [Serializable]
    public class SaveHeader
    {
        public int SlotNumber;
        public bool IsEmptySlot;
        public string FileName;
        public string TribeName;
        public string VillageName;
        public int HumanAmount;
        public int PassedDay;
        public DateTime SaveGameDateTime;
        public bool IsAutoSave;
        public int Cash;

        public static SaveHeader MakeEmptyHeader(int slotNumber, string saveFileName, bool isAutoSave)
        {
            return new SaveHeader() { SlotNumber = slotNumber, IsEmptySlot = true, FileName = saveFileName, IsAutoSave = isAutoSave };
        }
    }
