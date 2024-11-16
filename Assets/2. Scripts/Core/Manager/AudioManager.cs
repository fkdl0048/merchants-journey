using UnityEngine;

namespace Scripts.Manager
{
    public class AudioManager : Singleton<AudioManager>
    {
        private AudioSource bgmSource;
        private AudioSource sfxSource;
    
        protected override void Awake()
        {
            base.Awake();
            SetupAudioSources();
        }

        private void SetupAudioSources()
        {
            bgmSource = gameObject.AddComponent<AudioSource>();
            sfxSource = gameObject.AddComponent<AudioSource>();
        
            bgmSource.loop = true;
            sfxSource.loop = false;
        }

        public void PlayBGM(AudioClip clip, float volume = 1f)
        {
            bgmSource.clip = clip;
            bgmSource.volume = volume;
            bgmSource.Play();
        }

        public void PlaySFX(AudioClip clip, float volume = 1f)
        {
            sfxSource.PlayOneShot(clip, volume);
        }
    }
}