using System.Collections;
using Scripts.Manager;
using Scripts.Utils;
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
            Debug.Log("Initializing UpgradeUI");
            // SaveManager에서 게임 데이터 가져오기
            var gameData = SaveManager.Instance.GetGameData();
            
            // 기존 패널의 자식 오브젝트들 제거
            foreach (Transform child in PyoduUpgradePanel.transform)
            {
                Destroy(child.gameObject);
            }
            foreach (Transform child in PyosaUpgradePanel.transform)
            {
                Destroy(child.gameObject);
            }
            
            // 유닛 데이터를 타입별로 분류
            var pyoduUnits = gameData.ownedUnits.FindAll(unit => unit.unitType == UnitType.Pyodu);
            var pyosaUnits = gameData.ownedUnits.FindAll(unit => unit.unitType == UnitType.Pyosa);
            
            // Pyodu 유닛 패널 생성
            UpgradeUnitPanelUI firstPyoduPanel = null;
            foreach (var unitData in pyoduUnits)
            {
                var panel = Instantiate(upgradeUnitPanelPrefab, PyoduUpgradePanel.transform);
                var panelUI = panel.GetComponent<UpgradeUnitPanelUI>();
                panelUI.Initialize(UnitType.Pyodu);
                
                if (firstPyoduPanel == null)
                {
                    firstPyoduPanel = panelUI;
                }
            }
            
            // Pyosa 유닛 패널 생성
            UpgradeUnitPanelUI firstPyosaPanel = null;
            foreach (var unitData in pyosaUnits)
            {
                var panel = Instantiate(upgradeUnitPanelPrefab, PyosaUpgradePanel.transform);
                var panelUI = panel.GetComponent<UpgradeUnitPanelUI>();
                panelUI.Initialize(UnitType.Pyosa);
                
                if (firstPyosaPanel == null)
                {
                    firstPyosaPanel = panelUI;
                }
            }
            
            // Cargo 패널 가져오기 (씬에 이미 존재)
            var firstCargoPanel = CargoUpgradePanel.GetComponentInChildren<UpgradeUnitPanelUI>();
            
            // 기본 진입 시, Pyodu 버튼 활성화
            OnPyoduButtonClicked();
            
            // 다음 프레임에서 각 타입의 첫 번째 패널 선택
            StartCoroutine(SelectFirstPanelsNextFrame(firstPyoduPanel, firstPyosaPanel, firstCargoPanel));
        }
        
        private IEnumerator SelectFirstPanelsNextFrame(UpgradeUnitPanelUI firstPyoduPanel, UpgradeUnitPanelUI firstPyosaPanel, UpgradeUnitPanelUI firstCargoPanel)
        {
            yield return null; // 다음 프레임까지 대기
            
            if (firstPyoduPanel != null)
            {
                Debug.Log("Selecting first Pyodu panel");
                firstPyoduPanel.SelectButton();
            }
            
            if (firstPyosaPanel != null)
            {
                Debug.Log("Selecting first Pyosa panel");
                firstPyosaPanel.SelectButton();
            }
            
            if (firstCargoPanel != null)
            {
                Debug.Log("Selecting first Cargo panel");
                firstCargoPanel.SelectButton();
            }
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