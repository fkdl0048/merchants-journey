using UnityEngine;
using UnityEngine.UI;
using TMPro;
using _2._Scripts.Unit;
using System;
using Scripts.Utils;

namespace Scripts.UI.GameUISub
{
    public class UpgradeUnitPanelUI : MonoBehaviour
    {
        [Header("Select")]
        [SerializeField] private Button selectButton;
        [SerializeField] private GameObject selectBackground;
        [SerializeField] private TextMeshProUGUI unitNameText;
        
        [Header("Container")]
        [SerializeField] private Transform upgradeContainer;
        
        [Header("Prefabs")]
        [SerializeField] private GameObject upgradeButtonPrefab;
        [SerializeField] private GameObject itemButtonPrefab;

        private UnitType unitType;
        private UnitData unitData;
        private UpgradeUI upgradeUI;
        private bool isStatUpgradeVisible = false;
        
        public event Action<UpgradeUnitPanelUI> OnSelected;
        
        private void Awake()
        {
            selectButton.onClick.AddListener(OnSelectButtonClicked);
            selectBackground.SetActive(false);
        }

        public void Initialize(UnitType type, string unitName, UnitData data, UpgradeUI ui)
        {
            unitType = type;
            unitData = data;
            upgradeUI = ui;
            isStatUpgradeVisible = false;  // 상태 초기화
            
            unitNameText.text = unitName;
            CreateButtons();
        }

        public void UpdateUnitData(UnitData newData)
        {
            unitData = newData;
            ClearButtons();
            CreateButtons();
        }

        private void ClearButtons()
        {
            foreach (Transform child in upgradeContainer)
            {
                Destroy(child.gameObject);
            }
        }
        
        private void CreateButtons()
        {
            CreateUpgradeButton();
            if (unitType == UnitType.Pyodu)
            {
                CreateItemButton();
            }
        }

        private void CreateUpgradeButton()
        {
            var upgradeButton = Instantiate(upgradeButtonPrefab, upgradeContainer).GetComponent<Button>();
            var upgradeButtonText = upgradeButton.GetComponentInChildren<TextMeshProUGUI>();
            
            if (unitData.unitClass == UnitClass.None)
            {
                upgradeButtonText.text = "클래스 선택";
                upgradeButton.onClick.AddListener(() => upgradeUI.ShowClassButtons(unitData, upgradeButton));
            }
            else
            {
                upgradeButtonText.text = unitData.unitClass.ToString();
                upgradeButton.onClick.AddListener(OnUpgradeButtonClicked);
            }
        }

        private void CreateItemButton()
        {
            Instantiate(itemButtonPrefab, upgradeContainer);
        }

        private void OnUpgradeButtonClicked()
        {
            isStatUpgradeVisible = !isStatUpgradeVisible;
            
            if (isStatUpgradeVisible)
            {
                upgradeUI.InitializeStatUpgrades(unitType);
                upgradeUI.ToggleStatUpgrades(true);
            }
            else
            {
                upgradeUI.ToggleStatUpgrades(false);
            }
        }

        public void OnSelectButtonClicked()
        {
            var allPanels = transform.parent.GetComponentsInChildren<UpgradeUnitPanelUI>();
            transform.SetSiblingIndex(0);
            
            foreach (var panel in allPanels)
            {
                panel.SetSelected(panel == this);
                if (panel != this)
                {
                    panel.HideStatUpgrade();
                }
            }

            OnSelected?.Invoke(this);
        }

        public void HideStatUpgrade()
        {
            isStatUpgradeVisible = false;
            upgradeUI.ToggleStatUpgrades(false);
        }

        public void SetSelected(bool selected)
        {
            if (selectBackground != null)
            {
                selectBackground.SetActive(selected);
            }
        }

        public UnitData GetUnitData()
        {
            return unitData;
        }
    }
}
