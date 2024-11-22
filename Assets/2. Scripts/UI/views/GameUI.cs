using Scripts.Manager;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Scripts.UI
{
    public class GameUI : UIBase
    {
        [Header("Canvas")]
        [SerializeField] private Canvas canvas;
        
        [Header("Unit Placement UI")]
        [SerializeField] private GameObject unitPlacementPanel;
        [SerializeField] private Button placementCompleteButton;

        [Header("Wave UI")]
        [SerializeField] private GameObject wavePanel;
        [SerializeField] private TextMeshProUGUI waveText;
        [SerializeField] private Slider waveProgressBar;
        
        [Header("Game Clear UI")]
        [SerializeField] private GameObject gameClearPanel;
        [SerializeField] private Button nextStageButton;
        [SerializeField] private Button clearMenuButton;

        [Header("Game Over UI")]
        [SerializeField] private GameObject gameOverPanel;
        [SerializeField] private Button retryButton;
        [SerializeField] private Button gameOverMenuButton;

        [Header("Next State Button (Test)")]
        [SerializeField] private Button nextStateButton;
        
        [Header("Sound")]
        [SerializeField] private AudioClip buttonSound;

        // Events => 복잡해질 것 같아서 에디터에서 설정 가능하게 UnityAction으로 변경
        public UnityAction OnUnitPlacementComplete;
        public UnityAction OnRetryClick;
        public UnityAction OnMainMenuClick;
        public UnityAction OnNextStageClick;

        private void Awake()
        {
            if (canvas.gameObject.activeSelf == false)
            {
                canvas.gameObject.SetActive(true);
            }

            InitializeUI();
        }

        private void InitializeUI()
        {
            // delegate 초기화
            placementCompleteButton.onClick.AddListener(() => OnUnitPlacementComplete?.Invoke());
            retryButton.onClick.AddListener(() => OnRetryClick?.Invoke());
            nextStageButton.onClick.AddListener(() => OnNextStageClick?.Invoke());
            clearMenuButton.onClick.AddListener(() => OnMainMenuClick?.Invoke());
            gameOverMenuButton.onClick.AddListener(() => OnMainMenuClick?.Invoke());

            // 테스트용 NextState 버튼
            if (nextStateButton != null)
            {
                nextStateButton.onClick.AddListener(() => OnUnitPlacementComplete?.Invoke());
            }
            
            // sound
            placementCompleteButton.onClick.AddListener(() => AudioManager.Instance.PlaySFX(buttonSound));
            retryButton.onClick.AddListener(() => AudioManager.Instance.PlaySFX(buttonSound));
            nextStageButton.onClick.AddListener(() => AudioManager.Instance.PlaySFX(buttonSound));
            clearMenuButton.onClick.AddListener(() => AudioManager.Instance.PlaySFX(buttonSound));
            gameOverMenuButton.onClick.AddListener(() => AudioManager.Instance.PlaySFX(buttonSound));

            // 모든 패널 비활성화
            HideAllPanels();
        }

        private void HideAllPanels()
        {
            unitPlacementPanel.SetActive(false);
            wavePanel.SetActive(false);
            gameClearPanel.SetActive(false);
            gameOverPanel.SetActive(false);
        }

        // Unit Placement UI Methods
        public void ShowUnitPlacementUI()
        {
            HideAllPanels();
            unitPlacementPanel.SetActive(true);
            // TODO: 유닛 배치 UI 표시
            // 유닛 선택 버튼들을 활성화하고 각각에 OnUnitTypeSelected 이벤트 연결
        }

        public void HideUnitPlacementUI()
        {
            unitPlacementPanel.SetActive(false);
            // TODO: 유닛 배치 UI 숨기기
        }

        public void ShowWaveUI()
        {
            HideAllPanels();
            wavePanel.SetActive(true);
        }
        
        public void HideWaveUI()
        {
            wavePanel.SetActive(false);
        }

        public void ShowGameClearUI()
        {
            HideAllPanels();
            gameClearPanel.SetActive(true);
        }
        
        public void HideGameClearUI()
        {
            gameClearPanel.SetActive(false);
        }

        public void ShowGameOverUI()
        {
            HideAllPanels();
            gameOverPanel.SetActive(true);
        }
        
        public void HideGameOverUI()
        {
            gameOverPanel.SetActive(false);
        }
        
        private void OnDestroy()
        {
            placementCompleteButton.onClick.RemoveAllListeners();
            retryButton.onClick.RemoveAllListeners();
            nextStageButton.onClick.RemoveAllListeners();
            clearMenuButton.onClick.RemoveAllListeners();
            gameOverMenuButton.onClick.RemoveAllListeners();
        }
    }
}