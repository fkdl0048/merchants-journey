using System.Collections.Generic;
using UnityEngine;
using Scripts.Manager;
using Scripts.Utils;
using Scripts.UI.GameUISub;
using _2._Scripts.Unit;

namespace Scripts.UI.GameUISub.Controllers
{
    public class UnitPanelController
    {
        private GameObject upgradeUnitPanelPrefab;
        private Dictionary<UnitType, GameObject> panelContainers;
        private Dictionary<UnitType, List<UpgradeUnitPanelUI>> activePanels;
        private UpgradeUnitPanelUI selectedPanel;

        public UnitPanelController(GameObject prefab, Dictionary<UnitType, GameObject> containers)
        {
            upgradeUnitPanelPrefab = prefab;
            panelContainers = containers;
            activePanels = new Dictionary<UnitType, List<UpgradeUnitPanelUI>>();
            
            foreach (UnitType type in System.Enum.GetValues(typeof(UnitType)))
            {
                activePanels[type] = new List<UpgradeUnitPanelUI>();
            }
        }

        public void InitializePanels(UpgradeUI upgradeUI)
        {
            ClearAllPanels();
            
            var gameData = SaveManager.Instance.GetGameData();
            if (gameData?.ownedUnits == null) return;

            foreach (var unitData in gameData.ownedUnits)
            {
                if (panelContainers.TryGetValue(unitData.unitType, out var container))
                {
                    CreateUnitPanel(container, unitData, unitData.unitType, upgradeUI);
                }
            }
        }

        private void CreateUnitPanel(GameObject container, UnitData unitData, UnitType type, UpgradeUI upgradeUI)
        {
            var panel = Object.Instantiate(upgradeUnitPanelPrefab, container.transform);
            var panelUI = panel.GetComponent<UpgradeUnitPanelUI>();
            
            if (panelUI != null)
            {
                panelUI.Initialize(type, unitData.unitName, unitData, upgradeUI);
                panelUI.OnSelected += HandlePanelSelected;
                activePanels[type].Add(panelUI);
            }
        }

        private void HandlePanelSelected(UpgradeUnitPanelUI panel)
        {
            selectedPanel = panel;
        }

        public UpgradeUnitPanelUI GetSelectedPanel()
        {
            return selectedPanel;
        }

        public void SelectFirstPanel(UnitType type)
        {
            if (activePanels.TryGetValue(type, out var panels) && panels.Count > 0)
            {
                panels[0].OnSelectButtonClicked();
            }
        }

        public void UpdatePanelForUnit(UnitData unitData, UpgradeUI upgradeUI)
        {
            if (!panelContainers.TryGetValue(unitData.unitType, out var container)) return;

            var panels = activePanels[unitData.unitType];
            var existingPanel = panels.Find(p => p.GetUnitData().unitId == unitData.unitId);
            
            if (existingPanel != null)
            {
                // Update existing panel
                existingPanel.UpdateUnitData(unitData);
            }
            else
            {
                // Create new panel
                CreateUnitPanel(container, unitData, unitData.unitType, upgradeUI);
            }
        }

        private void ClearAllPanels()
        {
            foreach (var container in panelContainers.Values)
            {
                foreach (Transform child in container.transform)
                {
                    Object.Destroy(child.gameObject);
                }
            }
            
            foreach (var panels in activePanels.Values)
            {
                panels.Clear();
            }
            selectedPanel = null;
        }
    }
}
