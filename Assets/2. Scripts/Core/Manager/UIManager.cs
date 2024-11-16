using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Scripts.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Manager
{
    // 요거는 MVVM 패턴으로 사용하려고
    public class UIManager : MonoBehaviour
    {
        private static UIManager instance;
        public static UIManager Instance => instance;

        [Header("UI Panels")]
        [SerializeField] private GameObject mainMenuPanel;
        [SerializeField] private GameObject unitPlacementPanel;
        [SerializeField] private GameObject gameplayPanel;
        [SerializeField] private GameObject pausePanel;
        [SerializeField] private GameObject gameOverPanel;
        [SerializeField] private GameObject loadingScreen;

        [Header("Loading UI")]
        [SerializeField] private Slider loadingProgressBar;
        [SerializeField] private TMP_Text loadingText;

        [Header("Transition")]
        [SerializeField] private Animator transitionAnimator;
        [SerializeField] private float transitionDuration = 0.5f;

        private Stack<GameObject> uiStack = new Stack<GameObject>();

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
                InitializeUI();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void InitializeUI()
        {
            // 모든 패널 비활성화
            mainMenuPanel?.SetActive(false);
            unitPlacementPanel?.SetActive(false);
            gameplayPanel?.SetActive(false);
            pausePanel?.SetActive(false);
            gameOverPanel?.SetActive(false);
            loadingScreen?.SetActive(false);
        }

        private void Start()
        {
            GameManager.Instance.OnGameStateChanged += HandleGameStateChanged;
        }

        private void OnDestroy()
        {
            if (GameManager.Instance != null)
                GameManager.Instance.OnGameStateChanged -= HandleGameStateChanged;
        }

        public void ShowLoadingScreen(bool show)
        {
            if (loadingScreen != null)
            {
                loadingScreen.SetActive(show);
                if (show)
                {
                    loadingProgressBar.value = 0f;
                    loadingText.text = "Loading...";
                }
            }
        }

        public void UpdateLoadingProgress(float progress)
        {
            if (loadingProgressBar != null)
            {
                loadingProgressBar.value = progress;
                loadingText.text = $"Loading... {(progress * 100):F0}%";
            }
        }

        private async void HandleGameStateChanged(GameState newState)
        {
            await TransitionToNewState(newState);
        }

        private async Task TransitionToNewState(GameState newState)
        {
            if (transitionAnimator != null)
                transitionAnimator.SetTrigger("FadeOut");

            await Task.Delay(Mathf.RoundToInt(transitionDuration * 1000));

            HideAllPanels();
            ShowPanelForState(newState);

            if (transitionAnimator != null)
                transitionAnimator.SetTrigger("FadeIn");
        }

        private void HideAllPanels()
        {
            mainMenuPanel?.SetActive(false);
            unitPlacementPanel?.SetActive(false);
            gameplayPanel?.SetActive(false);
            pausePanel?.SetActive(false);
            gameOverPanel?.SetActive(false);
        }

        private void ShowPanelForState(GameState state)
        {
            GameObject targetPanel = GetTargetPanel(state);
            if (targetPanel != null)
            {
                targetPanel.SetActive(true);
                uiStack.Push(targetPanel);
            }
        }

        private GameObject GetTargetPanel(GameState state)
        {
            return state switch
            {
                GameState.MainMenu => mainMenuPanel,
                GameState.UnitPlacement => unitPlacementPanel,
                GameState.Playing => gameplayPanel,
                GameState.Paused => pausePanel,
                GameState.GameOver => gameOverPanel,
                _ => null
            };
        }

        public void ShowMessage(string message, float duration = 2f)
        {
            StartCoroutine(ShowMessageCoroutine(message, duration));
        }

        private IEnumerator ShowMessageCoroutine(string message, float duration)
        {
            // 메시지 UI 표시 로직
            yield return new WaitForSeconds(duration);
            // 메시지 UI 숨기기 로직
        }
    }
}