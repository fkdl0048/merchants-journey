using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Scripts.Data;
using UnityEngine;

namespace Scripts.Manager
{
    public class DataManager : MonoBehaviour
    {
        private static DataManager instance;
        public static DataManager Instance => instance;

        [Header("Game Settings")]
        public GameSettings gameSettings;
        public int initialCurrency = 1000;

        [Header("Game Data")]
        public List<UnitData> availableUnits = new List<UnitData>();
        public List<UnitData> placedUnits = new List<UnitData>();

        private int currentScore;
        private int currency;
        private int highScore;

        // Properties
        public int CurrentScore 
        { 
            get => currentScore;
            set
            {
                currentScore = value;
                OnScoreChanged?.Invoke(currentScore);
                if (currentScore > highScore)
                {
                    highScore = currentScore;
                    SaveHighScore();
                }
            }
        }

        public int Currency
        {
            get => currency;
            set
            {
                currency = value;
                OnCurrencyChanged?.Invoke(currency);
            }
        }

        // Events
        public event Action<int> OnScoreChanged;
        public event Action<int> OnCurrencyChanged;
        public event Action<UnitData> OnUnitPlaced;
        public event Action OnGameDataReset;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
                LoadGameSettings();
                LoadHighScore();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void ResetGameData()
        {
            CurrentScore = 0;
            Currency = initialCurrency;
            placedUnits.Clear();
            
            OnGameDataReset?.Invoke();
            //SaveGameData();
        }

        /// <summary>
        /// 저장이랑 세이브는 나중에
        /// </summary>
        // public async Task SaveGameData()
        // {
        //     try
        //     {
        //         GameSaveData saveData = new GameSaveData
        //         {
        //             score = CurrentScore,
        //             currency = Currency,
        //             placedUnitIds = placedUnits.Select(u => u.unitId).ToList()
        //         };
        //
        //         string json = JsonUtility.ToJson(saveData);
        //         await File.WriteAllTextAsync(GetSaveFilePath(), json);
        //     }
        //     catch (Exception e)
        //     {
        //         Debug.LogError($"Error saving game data: {e.Message}");
        //     }
        // }

        // public async Task LoadGameData()
        // {
        //     try
        //     {
        //         string path = GetSaveFilePath();
        //         if (File.Exists(path))
        //         {
        //             string json = await File.ReadAllTextAsync(path);
        //             GameSaveData saveData = JsonUtility.FromJson<GameSaveData>(json);
        //
        //             CurrentScore = saveData.score;
        //             Currency = saveData.currency;
        //             
        //             // 유닛 복원
        //             placedUnits = saveData.placedUnitIds
        //                 .Select(id => availableUnits.Find(u => u.unitId == id))
        //                 .Where(u => u != null)
        //                 .ToList();
        //         }
        //     }
        //     catch (Exception e)
        //     {
        //         Debug.LogError($"Error loading game data: {e.Message}");
        //     }
        // }

        private void LoadGameSettings()
        {
            if (PlayerPrefs.HasKey("GameSettings"))
            {
                string json = PlayerPrefs.GetString("GameSettings");
                JsonUtility.FromJsonOverwrite(json, gameSettings);
            }
        }

        public void SaveGameSettings()
        {
            string json = JsonUtility.ToJson(gameSettings);
            PlayerPrefs.SetString("GameSettings", json);
            PlayerPrefs.Save();
        }

        private void LoadHighScore()
        {
            highScore = PlayerPrefs.GetInt("HighScore", 0);
        }

        private void SaveHighScore()
        {
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
        }

        public bool AddUnit(UnitData unit)
        {
            if (Currency >= unit.cost)
            {
                placedUnits.Add(unit);
                Currency -= unit.cost;
                OnUnitPlaced?.Invoke(unit);
                return true;
            }
            return false;
        }

        private string GetSaveFilePath()
        {
            return Path.Combine(Application.persistentDataPath, "gamesave.json");
        }
    }
}