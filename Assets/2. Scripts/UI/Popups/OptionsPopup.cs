using Scripts.Manager;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI
{
    public class OptionsPopup : PopupBase
    {
        [Header("Audio Controls")]
        [SerializeField] private Slider bgmSlider;
        [SerializeField] private Slider sfxSlider;
        
        [Header("UI References")]
        [SerializeField] private RectTransform backgroundPanel;
    
        protected override void Initialize()
        {
            base.Initialize();
            if (backgroundPanel != null)
            {
                backgroundPanel.anchorMin = new Vector2(0.5f, 0.5f);
                backgroundPanel.anchorMax = new Vector2(0.5f, 0.5f);
            }
            SetupSliders();
        }
    
        private void SetupSliders()
        {
            // 현재 설정값 로드
            bgmSlider.value = PlayerPrefs.GetFloat("BGMVolume", 1f);
            sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);
        
            // 이벤트 설정
            bgmSlider.onValueChanged.AddListener(OnBGMVolumeChanged);
            sfxSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
        }
    
        private void OnBGMVolumeChanged(float value)
        {
            AudioManager.Instance.SetBGMVolume(value);
            PlayerPrefs.SetFloat("BGMVolume", value);
            PlayerPrefs.Save();
        }
    
        private void OnSFXVolumeChanged(float value)
        {
            AudioManager.Instance.SetSFXVolume(value);
            PlayerPrefs.SetFloat("SFXVolume", value);
            PlayerPrefs.Save();
        }
    
        protected override void OnClose()
        {
            // 팝업을 닫기 전에 설정 저장
            PlayerPrefs.Save();
            base.OnClose();
        }

        private void OnDestroy()
        {
            bgmSlider?.onValueChanged.RemoveAllListeners();
            sfxSlider?.onValueChanged.RemoveAllListeners();
        }

        public static OptionsPopup Show()
        {
            return UIManager.Instance.ShowPopup<OptionsPopup>("UI/Popups/OptionsPopup");
        }
    }
}