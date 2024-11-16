using UnityEngine;

namespace Scripts.Data
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "Game/Settings")]
    public class GameSettings : ScriptableObject
    {
        [Range(0f, 1f)]
        public float bgmVolume = 1f;

        [Range(0f, 1f)]
        public float sfxVolume = 1f;

        public int targetFrameRate = 60;

        public void SetMusicVolume(float volume)
        {
            bgmVolume = Mathf.Clamp(volume, 0f, 1f);
        }

        public void SetSfxVolume(float volume)
        {
            sfxVolume = Mathf.Clamp(volume, 0f, 1f);
        }
    }
}