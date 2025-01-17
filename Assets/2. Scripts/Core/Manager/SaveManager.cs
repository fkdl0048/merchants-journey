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
        
        // stage 정보를 가져오기 위한 프로퍼티
        public int CurrentStage => gameData.currentStage;

        protected override void Awake()
        {
            LoadGameData();
        }

        private GameData CreateNewGameData()
        {
            var newGameData = new GameData
            {
                currentStage = 1,  // 명시적으로 스테이지 1로 설정
                gold = 1000,       // 초기 골드도 명시적으로 설정
                ownedUnits = new List<UnitData>()
            };
            
            // 초기 유닛 생성
            newGameData.ownedUnits.Add(new UnitData("unit_1", "만득이", UnitType.Pyosa, UnitClass.None));
            newGameData.ownedUnits.Add(new UnitData("unit_2", "두칠이", UnitType.Pyosa, UnitClass.None));
            newGameData.ownedUnits.Add(new UnitData("unit_3", "강철이", UnitType.Pyosa, UnitClass.None));
            newGameData.ownedUnits.Add(new UnitData("unit_4", "표두표두", UnitType.Pyodu, UnitClass.None));
            return newGameData;
        }

        private void LoadGameData()
        {
#if UNITY_EDITOR
            gameData = LoadData<GameData>(GAME_DATA_KEY);
            if (gameData == null)
            {
                gameData = CreateNewGameData();
                SaveGameData(gameData);
            }
#else
            gameData = LoadData<GameData>(GAME_DATA_KEY);
            if (gameData == null)
            {
                gameData = CreateNewGameData();
                SaveGameData(gameData);
            }
#endif
            Debug.Log($"LoadGameData - Current Stage: {gameData.currentStage}");
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
            Debug.Log($"??????????????{data.currentStage}");
#if !UNITY_EDITOR
            SaveData(GAME_DATA_KEY, data);
#endif
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
            var newGameData = CreateNewGameData();
            SaveGameDataToSlot(slotIndex, newGameData);  // 먼저 슬롯에 저장
            SaveGameData(newGameData);  // 그 다음 현재 게임 데이터로 설정
        }

        public void DeleteGameDataFromSlot(int slotIndex)
        {
            string key = $"{GAME_DATA_KEY}_{slotIndex}";
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

        public void DeleteData(string key)
        {
            PlayerPrefs.DeleteKey(key);
        }
    }
}