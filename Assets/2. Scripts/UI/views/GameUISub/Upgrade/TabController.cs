using UnityEngine;
using UnityEngine.UI;
using _2._Scripts.Unit;
using Scripts.Utils;

namespace Scripts.UI.GameUISub.Controllers
{
    public class TabController
    {
        private readonly Button pyoduButton;
        private readonly Button pyosaButton;
        private readonly Button cargoButton;
        private readonly GameObject pyoduPanel;
        private readonly GameObject pyosaPanel;
        private readonly GameObject cargoPanel;
        private readonly UnitPanelController panelController;
        private readonly ClassSelectionController classController;

        public TabController(
            Button pyoduBtn, Button pyosaBtn, Button cargoBtn,
            GameObject pyoduPnl, GameObject pyosaPnl, GameObject cargoPnl,
            UnitPanelController panelCtrl, ClassSelectionController classCtrl)
        {
            pyoduButton = pyoduBtn;
            pyosaButton = pyosaBtn;
            cargoButton = cargoBtn;
            pyoduPanel = pyoduPnl;
            pyosaPanel = pyosaPnl;
            cargoPanel = cargoPnl;
            panelController = panelCtrl;
            classController = classCtrl;

            SetupTabButtons();
        }

        private void SetupTabButtons()
        {
            pyoduButton.onClick.AddListener(() => SwitchTab(UnitType.Pyodu));
            pyosaButton.onClick.AddListener(() => SwitchTab(UnitType.Pyosa));
            cargoButton.onClick.AddListener(() => SwitchTab(UnitType.Cargo));
        }

        public void SwitchTab(UnitType type)
        {
            classController.HideClassSelection();
            
            SetButtonsInteractable(type);
            SetPanelsActive(type);
            panelController.SelectFirstPanel(type);
        }

        private void SetButtonsInteractable(UnitType type)
        {
            pyoduButton.interactable = type != UnitType.Pyodu;
            pyosaButton.interactable = type != UnitType.Pyosa;
            cargoButton.interactable = type != UnitType.Cargo;
        }

        private void SetPanelsActive(UnitType type)
        {
            pyoduPanel.SetActive(type == UnitType.Pyodu);
            pyosaPanel.SetActive(type == UnitType.Pyosa);
            cargoPanel.SetActive(type == UnitType.Cargo);
        }
    }
}
