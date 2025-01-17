using Scripts.Manager;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI
{
    public class OptionsPopup : PopupBase
    {
        
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
        
            // 이벤트 설정
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
    
        private void OnEnable()
        {
            Time.timeScale = 0f; // 게임 일시정지
        }

        public override void OnClose()
        {
            // 팝업을 닫기 전에 설정 저장
            PlayerPrefs.Save();
            Time.timeScale = 1f; // 게임 재개
            base.OnClose();
            Destroy(gameObject); // 팝업 객체 제거
        }

        private void OnDestroy()
        {
            
        }

        public static OptionsPopup Show()
        {
            return UIManager.Instance.ShowPopup<OptionsPopup>("UI/Popups/OptionsPopup");
        }
    }
}