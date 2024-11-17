using UnityEngine;

namespace Scripts.Manager
{
    public class SaveManager : Singleton<SaveManager>
    {
        public void SaveData<T>(string key, T data)
        {
            string json = JsonUtility.ToJson(data);
            PlayerPrefs.SetString(key, json);
            PlayerPrefs.Save();
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

        public void DeleteData(string key)
        {
            PlayerPrefs.DeleteKey(key);
        }
    }
}