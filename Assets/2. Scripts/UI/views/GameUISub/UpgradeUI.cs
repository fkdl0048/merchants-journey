using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using _2._Scripts.Unit;
using Scripts.UI.GameUISub.Controllers;
using System.Collections.Generic;
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
        
        [Header("Utility")]
        [SerializeField] private Button backButton;
        
        [Header("Upgrade Unit Panel (Prefab)")]
        [SerializeField] private GameObject upgradeUnitPanelPrefab;
        
        public UnityAction OnBackClicked;

        private UnitPanelController panelController;
        private ClassSelectionController classController;
        private TabController tabController;

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