using UnityEngine;
using UnityEngine.UI;
using _2._Scripts.Unit;
using Scripts.Manager;
using Scripts.Utils;

namespace Scripts.UI.GameUISub.Controllers
{
    public class ClassSelectionController
    {
        private readonly GameObject selectionContainer;
        private readonly UnitPanelController panelController;
        
        private UnitData currentSelectedUnit;
        private Button currentUpgradeButton;

        public ClassSelectionController(GameObject container, UnitPanelController controller)
        {
            selectionContainer = container;
            panelController = controller;
            HideClassSelection();
        }

        public void ShowClassButtons(UnitData unitData, Button upgradeButton)
        {
            if (currentUpgradeButton == upgradeButton && selectionContainer.activeSelf)
            {
                HideClassSelection();
            }
            else
            {
                currentSelectedUnit = unitData;
                currentUpgradeButton = upgradeButton;
                selectionContainer.SetActive(true);
            }
        }

        public void OnClassSelected(UnitClass classType)
        {
            if (currentSelectedUnit == null) return;

            currentSelectedUnit.unitClass = classType;
            SaveManager.Instance.SaveGameData(SaveManager.Instance.GetGameData());
            
            panelController.UpdatePanelForUnit(currentSelectedUnit, null);
            HideClassSelection();
        }

        public void HideClassSelection()
        {
            selectionContainer.SetActive(false);
            currentSelectedUnit = null;
            currentUpgradeButton = null;
        }
    }
}
