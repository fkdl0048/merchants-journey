using UnityEngine;

namespace Scripts.Manager
{
    public class AudioManager : Singleton<AudioManager>
    {
        private AudioSource bgmSource;
        private AudioSource sfxSource;
        private float bgmVolume = 1f;
        private float sfxVolume = 1f;
        private string currentBGMPath;
    
        protected override void Awake()
        {
            base.Awake();
            SetupAudioSources();
            LoadVolumes();
        }
        
        private void SetupAudioSources()
        {
            bgmSource = gameObject.AddComponent<AudioSource>();
            bgmSource.loop = true;
            bgmSource.playOnAwake = false;
            
            sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSource.loop = false;
            sfxSource.playOnAwake = false;
        }
        
        private void LoadVolumes()
        {
            bgmVolume = PlayerPrefs.GetFloat("BGMVolume", 1f);
            sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
            
            bgmSource.volume = bgmVolume;
            sfxSource.volume = sfxVolume;
        }

        public void PlayBGM(string path)
        {
            if (currentBGMPath == path && bgmSource.isPlaying) return;
            
            AudioClip clip = Resources.Load<AudioClip>(path);
            if (clip == null)
            {
                Debug.LogWarning($"BGM not found at path: {path}");
                return;
            }

            currentBGMPath = path;
            PlayBGM(clip);
        }
        
        public void PlayBGM(AudioClip clip)
        {
            if (clip == null) return;
            
            bgmSource.clip = clip;
            bgmSource.volume = bgmVolume;
            bgmSource.Play();
        }

        public void PlaySFX(string path)
        {
            AudioClip clip = Resources.Load<AudioClip>(path);
            if (clip == null)
            {
                Debug.LogWarning($"SFX not found at path: {path}");
                return;
            }
            
            PlaySFX(clip);
        }
        
        public void PlaySFX(AudioClip clip)
        {
            if (clip == null) return;
            
            sfxSource.PlayOneShot(clip, sfxVolume);
        }
        
        public void SetBGMVolume(float volume)
        {
            bgmVolume = Mathf.Clamp01(volume);
            bgmSource.volume = bgmVolume;
            PlayerPrefs.SetFloat("BGMVolume", bgmVolume);
            PlayerPrefs.Save();
        }
        
        public void SetSFXVolume(float volume)
        {
            sfxVolume = Mathf.Clamp01(volume);
            sfxSource.volume = sfxVolume;
            PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
            PlayerPrefs.Save();
        }
        
        public void StopBGM()
        {
            bgmSource.Stop();
        }
        
        public void PauseBGM()
        {
            bgmSource.Pause();
        }
        
        public void ResumeBGM()
        {
            bgmSource.UnPause();
        }

        public string GetCurrentBGMPath()
        {
            return currentBGMPath;
        }
    }
}