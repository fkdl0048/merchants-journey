using Scripts.Manager;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI
{
    public class OptionsPopup : PopupBase
    {
        [SerializeField] private Slider bgmSlider;
        [SerializeField] private Slider sfxSlider;
    
        protected override void Awake()
        {
            base.Awake();
            SetupSliders();
        }
    
        private void SetupSliders()
        {
            // 저장된 값 불러오기
            bgmSlider.value = PlayerPrefs.GetFloat("BGMVolume", 1f);
            sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);
        
            // 이벤트 설정
            bgmSlider.onValueChanged.AddListener(OnBGMVolumeChanged);
            sfxSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
        }
    
        private void OnBGMVolumeChanged(float value)
        {
            PlayerPrefs.SetFloat("BGMVolume", value);
            AudioManager.Instance.SetBGMVolume(value);
        }
    
        private void OnSFXVolumeChanged(float value)
        {
            PlayerPrefs.SetFloat("SFXVolume", value);
            AudioManager.Instance.SetSFXVolume(value);
        }
    
        private void OnDestroy()
        {
            bgmSlider?.onValueChanged.RemoveAllListeners();
            sfxSlider?.onValueChanged.RemoveAllListeners();
            PlayerPrefs.Save();
        }
        
    }
}