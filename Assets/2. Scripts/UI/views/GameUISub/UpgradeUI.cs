using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Scripts.UI.GameUISub
{
    public class UpgradeUI : MonoBehaviour
    {
        [Header("Upgrade List (Button)")]
        [SerializeField] private Button PyoduButton;
        [SerializeField] private Button PyosaButton;
        [SerializeField] private Button CargoButton;
        [SerializeField] private GameObject PyoduUpgradePanel;
        [SerializeField] private GameObject PyosaUpgradePanel;
        [SerializeField] private GameObject CargoUpgradePanel;
        
        [Header("Utility")]
        [SerializeField] private Button backButton;
        
        [Header("Upgrade Unit Panel (Prefab)")]
        [SerializeField] private GameObject upgradeUnitPanelPrefab;
        
        public UnityAction OnBackClicked;
        
        // 1회 초기화
        private void Awake()
        {
            PyoduButton.onClick.AddListener(OnPyoduButtonClicked);
            PyosaButton.onClick.AddListener(OnPyosaButtonClicked);
            CargoButton.onClick.AddListener(OnCargoButtonClicked);
            backButton.onClick.AddListener(OnBackButtonClicked);
        }

        // Upgrade State 진입 시
        public void Initialized()
        {
            // 기본 진입 시, Pyodu 버튼 활성화
            OnPyoduButtonClicked();
        }
        
        // Pyodu 버튼 클릭
        private void OnPyoduButtonClicked()
        {
            PyoduUpgradePanel.SetActive(true);
            PyosaUpgradePanel.SetActive(false);
            CargoUpgradePanel.SetActive(false);
        }
        
        // Pyosa 버튼 클릭
        private void OnPyosaButtonClicked()
        {
            PyoduUpgradePanel.SetActive(false);
            PyosaUpgradePanel.SetActive(true);
            CargoUpgradePanel.SetActive(false);
        }
        
        // Cargo 버튼 클릭
        private void OnCargoButtonClicked()
        {
            PyoduUpgradePanel.SetActive(false);
            PyosaUpgradePanel.SetActive(false);
            CargoUpgradePanel.SetActive(true);
        }
        
        // 뒤로 가기 버튼 클릭
        private void OnBackButtonClicked()
        {
            OnBackClicked?.Invoke();
        }
        
    }
}