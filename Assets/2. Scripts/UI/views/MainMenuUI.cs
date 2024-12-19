using Scripts.Manager;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Scripts.UI
{
    public class MainMenuUI : UIBase
    {
        [Header("Buttons")]
        [SerializeField] private Button startButton;
        [SerializeField] private Button optionsButton;
        [SerializeField] private Button creditsButton;
        [SerializeField] private Button quitButton;
        
        [Header("Sound")]
        [SerializeField] private AudioClip startButtonSound;
        [SerializeField] private AudioClip ButtonSound;
        
        protected override void Initialize()
        {
            base.Initialize();
            SetupButtons();
        }
    
        private void SetupButtons()
        {
            startButton?.onClick.AddListener(OnStartClick);
            optionsButton?.onClick.AddListener(OnOptionsClick);
            creditsButton?.onClick.AddListener(OnCreditsClick);
            quitButton?.onClick.AddListener(OnQuitClick);
            
            startButton?.onClick.AddListener(() => AudioManager.Instance.PlaySFX(startButtonSound));
            optionsButton?.onClick.AddListener(() => AudioManager.Instance.PlaySFX(ButtonSound));
        }
    
        private void OnStartClick()
        {
            SceneManager.LoadScene("InGame");
        }
    
        private void OnOptionsClick()
        {
            UIManager.Instance.ShowPopup<OptionsPopup>("UI/Popups/OptionsPopup");
        }

        private void OnCreditsClick()
        { 
            // credits UI
        }        
    
        private void OnQuitClick()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    
        private void OnDestroy()
        {
            startButton?.onClick.RemoveAllListeners();
            optionsButton?.onClick.RemoveAllListeners();
            quitButton?.onClick.RemoveAllListeners();
        }
    }
}