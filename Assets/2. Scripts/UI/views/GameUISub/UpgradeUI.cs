using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using _2._Scripts.Unit;
using Scripts.UI.GameUISub.Controllers;
using System.Collections.Generic;
using Scripts.Data;
using Scripts.Utils;

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
        
        [Header("Class Upgrade List")]
        [SerializeField] private GameObject UpgradeButtonContainer;
        [SerializeField] private Button SwordButton;
        [SerializeField] private Button LanceButton;
        [SerializeField] private Button BowButton;
        [SerializeField] private Button MartialArtsButton;
        
        [Header("Stat Upgrade List")]
        [SerializeField] private GameObject StatUpgradeContainer;
        [SerializeField] private GameObject upgradeElementPrefab;
        [SerializeField] private StatUpgradeData statUpgradeData;
        
        [Header("Utility")]
        [SerializeField] private Button backButton;
        
        [Header("Upgrade Unit Panel (Prefab)")]
        [SerializeField] private GameObject upgradeUnitPanelPrefab;
        
        public UnityAction OnBackClicked;

        private UnitPanelController panelController;
        private ClassSelectionController classController;
        private TabController tabController;

        private List<UpgradeElementUI> upgradeElements = new List<UpgradeElementUI>();

        private void Awake()
        {
            InitializeControllers();
            SetupClassButtons();
            backButton.onClick.AddListener(OnBackButtonClicked);
        }

        private void InitializeControllers()
        {
            var containers = new Dictionary<UnitType, GameObject>
            {
                { UnitType.Pyodu, PyoduUpgradePanel },
                { UnitType.Pyosa, PyosaUpgradePanel },
                { UnitType.Cargo, CargoUpgradePanel }
            };
            
            panelController = new UnitPanelController(upgradeUnitPanelPrefab, containers);
            classController = new ClassSelectionController(UpgradeButtonContainer, panelController);
            tabController = new TabController(
                PyoduButton, PyosaButton, CargoButton,
                PyoduUpgradePanel, PyosaUpgradePanel, CargoUpgradePanel,
                panelController, classController);
        }

        private void SetupClassButtons()
        {
            SwordButton.onClick.AddListener(() => classController.OnClassSelected(UnitClass.Sword));
            LanceButton.onClick.AddListener(() => classController.OnClassSelected(UnitClass.Lance));
            BowButton.onClick.AddListener(() => classController.OnClassSelected(UnitClass.Bow));
            MartialArtsButton.onClick.AddListener(() => classController.OnClassSelected(UnitClass.MartialArts));
        }

        public void Initialize()
        {
            Debug.Log("Initializing UpgradeUI");
            panelController.InitializePanels(this);
            tabController.SwitchTab(UnitType.Pyodu);
        }

        public void InitializeStatUpgrades(UnitType unitType)
        {
            ClearStatUpgrades();
            
            var upgrades = statUpgradeData.statUpgrades.FindAll(x => x.UnitType == unitType);
            foreach (var upgrade in upgrades)
            {
                GameObject go = Instantiate(upgradeElementPrefab, StatUpgradeContainer.transform);
                var element = go.GetComponent<UpgradeElementUI>();
                
                element.Initialize(
                    upgrade.StatType.ToString(), 
                    upgrade.Description,
                    upgrade.InitialValue,
                    upgrade.MaxValue,
                    upgrade.UpgradeCost
                );
                upgradeElements.Add(element);
            }
        }

        private void ClearStatUpgrades()
        {
            foreach (var element in upgradeElements)
            {
                if (element != null)
                    Destroy(element.gameObject);
            }
            upgradeElements.Clear();
        }

        public void ToggleStatUpgrades(bool show)
        {
            foreach (var element in upgradeElements)
            {
                if (element != null)
                    element.SetVisible(show);
            }
        }

        public void ShowClassButtons(UnitData unitData, Button upgradeButton)
        {
            classController.ShowClassButtons(unitData, upgradeButton);
        }

        private void OnBackButtonClicked()
        {
            OnBackClicked?.Invoke();
        }
    }
}