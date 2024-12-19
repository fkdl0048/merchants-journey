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

        private void LoadGameData()
        {
#if UNITY_EDITOR
            // 에디터에서는 매번 새로운 데이터 생성
            gameData = new GameData();
            // 초기 유닛 생성
            gameData.ownedUnits.Add(new UnitData("unit_bowman_1", "표사", UnitType.Pyosa, UnitClass.None));
            gameData.ownedUnits.Add(new UnitData("unit_bowman_2", "표사", UnitType.Pyosa, UnitClass.None));
            gameData.ownedUnits.Add(new UnitData("unit_captain_1", "표두", UnitType.Pyodu, UnitClass.None));
#else
            // 빌드에서는 저장된 데이터 로드
            gameData = LoadData<GameData>(GAME_DATA_KEY);
            if (gameData == null)
            {
                gameData = new GameData();
                // 초기 유닛 생성
                gameData.ownedUnits.Add(new UnitData("unit_bowman_1", "표사", UnitType.Bowman, UnitClass.Common));
                gameData.ownedUnits.Add(new UnitData("unit_bowman_2", "표사", UnitType.Bowman, UnitClass.Common));
                gameData.ownedUnits.Add(new UnitData("unit_captain_1", "표두", UnitType.Captain, UnitClass.Common));
                SaveGameData(gameData);
            }
#endif
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
#if !UNITY_EDITOR
            // 에디터에서는 저장하지 않음 => 빌드만 테스트를 위해 사용
            SaveData(GAME_DATA_KEY, data);
#endif
        }

        public T LoadData<T>(string key) where T : new()
        {
            if (PlayerPrefs.HasKey(key))
            {
                string json = PlayerPrefs.GetString(key);
                return JsonUtility.FromJson<T>(json);
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