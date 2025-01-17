using System;
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
        private DateTime sessionStartTime;  // 현재 게임 세션의 시작 시간
        
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
                ownedUnits = new List<UnitData>(),
                playTime = 0f,
                lastSaveTime = DateTime.Now
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
                sessionStartTime = DateTime.Now;  // 게임 로드시 세션 시작 시간 기록
            }
            else
            {
                gameData = CreateNewGameData();
            }
            Debug.Log($"LoadGameData - Current Stage: {gameData.currentStage}, Slot: {currentSlot}");
        }

        private void UpdatePlayTime()
        {
            if (gameData != null)
            {
                TimeSpan sessionDuration = DateTime.Now - sessionStartTime;
                gameData.playTime += (float)sessionDuration.TotalSeconds;
                sessionStartTime = DateTime.Now;  // 시간 업데이트 후 세션 시작 시간 리셋
            }
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

        public void SaveGameDataToSlot(int slot, GameData data)
        {
            UpdatePlayTime();  // 저장하기 전에 플레이 타임 업데이트
            data.lastSaveTime = DateTime.Now;  // 저장 시간 업데이트
            string json = JsonUtility.ToJson(data);
            PlayerPrefs.SetString($"{GAME_DATA_KEY}_{slot}", json);
            PlayerPrefs.Save();
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