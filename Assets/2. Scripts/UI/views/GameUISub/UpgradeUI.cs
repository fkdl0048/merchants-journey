using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using _2._Scripts.Unit;
using Scripts.UI.GameUISub.Controllers;
using System.Collections.Generic;
using Scripts.Data;
using Scripts.Manager;
using Scripts.Utils;
using TMPro;

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
        [SerializeField] private TextMeshProUGUI goldText;
        
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
            UpdateGoldText();
        }

        public void InitializeStatUpgrades(UnitType unitType)
        {
            ClearStatUpgrades();
            
            if (statUpgradeData == null || StatUpgradeContainer == null || upgradeElementPrefab == null)
            {
                Debug.LogError($"Required references are missing in UpgradeUI! statUpgradeData: {statUpgradeData}, StatUpgradeContainer: {StatUpgradeContainer}, upgradeElementPrefab: {upgradeElementPrefab}");
                return;
            }

            Debug.Log($"Finding upgrades for unit type: {unitType}");
            var upgrades = statUpgradeData.statUpgrades.FindAll(x => x.UnitType == unitType);
            Debug.Log($"Found {upgrades.Count} upgrades");

            foreach (var upgrade in upgrades)
            {
                if (upgrade == null)
                {
                    Debug.LogError("Upgrade info is null!");
                    continue;
                }

                Debug.Log($"Creating upgrade element for {upgrade.StatType}");
                GameObject go = Instantiate(upgradeElementPrefab, StatUpgradeContainer.transform);
                var element = go.GetComponent<UpgradeElementUI>();
                
                if (element != null)
                {
                    try
                    {
                        Debug.Log($"Initializing upgrade element - StatType: {upgrade.StatType}, Description: {upgrade.Description}, MinValue: {upgrade.MinValue}, MaxValue: {upgrade.MaxValue}, Cost: {upgrade.UpgradeCost}");
                        element.Initialize(
                            upgrade.StatType.ToString(),
                            upgrade.Description ?? "No Description",
                            upgrade.MinValue,
                            upgrade.MaxValue,
                            upgrade.UpgradeCost,
                            () => UpdateGoldText()
                        );
                        upgradeElements.Add(element);
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogError($"Error initializing upgrade element: {e.Message}\nStack trace: {e.StackTrace}");
                        Destroy(go);
                    }
                }
                else
                {
                    Debug.LogError("UpgradeElementUI component not found on instantiated prefab!");
                    Destroy(go);
                }
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

        private void UpdateGoldText()
        {
            if (goldText != null && SaveManager.Instance != null)
            {
                GameData gameData = SaveManager.Instance.GetGameData();
                goldText.text = $"{gameData.gold}";
            }
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