using System.Collections.Generic;
using _2._Scripts.Unit;
using UnityEngine;
using Scripts.Data;
using Scripts.Utils;

namespace Scripts.Manager
{
    public class SaveManager : Singleton<SaveManager>
    {
        private const string GAME_DATA_KEY = "GameData";
        private GameData gameData;
        private int currentSlot = -1;  // 현재 사용 중인 슬롯 (-1은 슬롯이 선택되지 않은 상태)
        
        // stage 정보를 가져오기 위한 프로퍼티
        public int CurrentStage => gameData?.currentStage ?? 1;
        public int CurrentSlot => currentSlot;

        protected override void Awake()
        {
            base.Awake();
            LoadGameData();
        }

        private GameData CreateNewGameData()
        {
            var newGameData = new GameData
            {
                currentStage = 1,
                gold = 1000,
                ownedUnits = new List<UnitData>()
            };
            
            newGameData.ownedUnits.Add(new UnitData("unit_1", "만득이", UnitType.Pyosa, UnitClass.None));
            newGameData.ownedUnits.Add(new UnitData("unit_2", "두칠이", UnitType.Pyosa, UnitClass.None));
            newGameData.ownedUnits.Add(new UnitData("unit_3", "강철이", UnitType.Pyosa, UnitClass.None));
            newGameData.ownedUnits.Add(new UnitData("unit_4", "표두표두", UnitType.Pyodu, UnitClass.None));
            return newGameData;
        }

        private void LoadGameData()
        {
            if (currentSlot >= 0)
            {
                gameData = LoadGameDataBySlot(currentSlot);
                if (gameData == null)
                {
                    gameData = CreateNewGameData();
                    SaveGameDataToSlot(currentSlot, gameData);
                }
            }
            else
            {
                gameData = CreateNewGameData();
            }
            Debug.Log($"LoadGameData - Current Stage: {gameData.currentStage}, Slot: {currentSlot}");
        }

        public GameData GetGameData()
        {
            if (gameData == null)
            {
                LoadGameData();
            }
            return gameData;
        }

        public void SaveGameData(GameData data)
        {
            gameData = data;
            if (currentSlot >= 0)
            {
                SaveGameDataToSlot(currentSlot, data);
                Debug.Log($"SaveGameData - Stage: {data.currentStage}, Slot: {currentSlot}");
            }
        }

        public bool HasSavedGame(int slotIndex)
        {
            string key = $"{GAME_DATA_KEY}_{slotIndex}";
            return PlayerPrefs.HasKey(key);
        }

        public GameData LoadGameDataBySlot(int slotIndex)
        {
            string key = $"{GAME_DATA_KEY}_{slotIndex}";
            return LoadData<GameData>(key);
        }

        public void SaveGameDataToSlot(int slotIndex, GameData data)
        {
            string key = $"{GAME_DATA_KEY}_{slotIndex}";
            SaveData(key, data);
        }

        public void CreateAndSaveNewGame(int slotIndex)
        {
            currentSlot = slotIndex;  // 현재 슬롯 설정
            var newGameData = CreateNewGameData();
            SaveGameDataToSlot(slotIndex, newGameData);
            SaveGameData(newGameData);
            Debug.Log($"CreateAndSaveNewGame - Stage: {newGameData.currentStage}, Slot: {currentSlot}");
        }

        public void LoadGameFromSlot(int slotIndex)
        {
            currentSlot = slotIndex;  // 현재 슬롯 설정
            var loadedData = LoadGameDataBySlot(slotIndex);
            SaveGameData(loadedData);
            Debug.Log($"LoadGameFromSlot - Stage: {loadedData.currentStage}, Slot: {currentSlot}");
        }

        public void DeleteGameDataFromSlot(int slotIndex)
        {
            string key = $"{GAME_DATA_KEY}_{slotIndex}";
            if (currentSlot == slotIndex)
            {
                currentSlot = -1;  // 현재 슬롯이 삭제되면 초기화
                gameData = CreateNewGameData();
            }
            PlayerPrefs.DeleteKey(key);
            PlayerPrefs.Save();
        }

        public T LoadData<T>(string key) where T : new()
        {
            if (PlayerPrefs.HasKey(key))
            {
                string json = PlayerPrefs.GetString(key);
                var data = JsonUtility.FromJson<T>(json);
                if (data == null)
                {
                    Debug.LogWarning($"Failed to deserialize data for key: {key}");
                    return new T();
                }
                return data;
            }
            return new T();
        }

        public void SaveData<T>(string key, T data)
        {
            string json = JsonUtility.ToJson(data);
            PlayerPrefs.SetString(key, json);
            PlayerPrefs.Save();
        }
    }
}