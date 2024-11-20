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
            placementCompleteButton.onClick.AddListener(() => OnUnitPlacementComplete?.Invoke());
            retryButton.onClick.AddListener(() => OnRetryClick?.Invoke());
            nextStageButton.onClick.AddListener(() => OnNextStageClick?.Invoke());
            clearMenuButton.onClick.AddListener(() => OnMainMenuClick?.Invoke());
            gameOverMenuButton.onClick.AddListener(() => OnMainMenuClick?.Invoke());

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
        }

        public void HideUnitPlacementUI()
        {
            unitPlacementPanel.SetActive(false);
        }

        // 다른 UI 메서드들..
    }
}