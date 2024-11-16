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
        [SerializeField] private Button quitButton;
    
        protected override void Initialize()
        {
            base.Initialize();
            SetupButtons();
        }
    
        private void SetupButtons()
        {
            startButton?.onClick.AddListener(OnStartClick);
            optionsButton?.onClick.AddListener(OnOptionsClick);
            quitButton?.onClick.AddListener(OnQuitClick);
        }
    
        private void OnStartClick()
        {
            SceneManager.LoadScene("InGame");
        }
    
        private void OnOptionsClick()
        {
            UIManager.Instance.ShowPopup<OptionsPopup>("UI/Popups/OptionsPopup");
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