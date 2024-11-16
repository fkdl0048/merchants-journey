using Scripts.Manager;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI
{
    public class MainMenuUI : UIBase
    {
        [SerializeField] private Button startButton;
        [SerializeField] private Button optionsButton;
        [SerializeField] private Button quitButton;
    
        protected override void Awake()
        {
            base.Awake();
            SetupButtons();
        }
    
        private void SetupButtons()
        {
            startButton?.onClick.AddListener(OnStartGame);
            optionsButton?.onClick.AddListener(OnOpenOptions);
            quitButton?.onClick.AddListener(OnQuitGame);
        }
    
        private void OnStartGame()
        {
            // 게임 시작 로직
            Debug.Log("Game Started");
        }
    
        private void OnOpenOptions()
        {
            UIManager.Instance.ShowPopup<OptionsPopup>("UI/Popups/OptionsPopup");
        }
    
        private void OnQuitGame()
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