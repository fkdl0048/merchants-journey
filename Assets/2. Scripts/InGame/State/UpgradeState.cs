using Scripts.Controller;
using Scripts.InGame.System;
using Scripts.Interface;
using Scripts.UI;
using Scripts.UI.GameUISub;
using Scripts.Utils;
using UnityEngine;

namespace Scripts.InGame.State
{
    public class UpgradeState : IInGameState
    {
        private readonly InGameSceneController controller;
        private readonly GameUI gameUI;
        private readonly UnitSystem unitSystem;
        private readonly UpgradeUI upgradeUI;
        
        public UpgradeState(InGameSceneController controller, GameUI gameUI, UnitSystem unitSystem)
        {
            this.controller = controller;
            this.gameUI = gameUI;
            this.unitSystem = unitSystem;

            upgradeUI = gameUI.GetUpgradeUI();
        }
        
        public void Enter()
        {
            gameUI.ShowUpgradeUI();
            
            upgradeUI.Initialize();
            
            upgradeUI.OnBackClicked += HandleBackClicked;
        }

        public void Update()
        {
            
        }

        public void Exit()
        {
            upgradeUI.OnBackClicked -= HandleBackClicked;
        }
        
        private void HandleBackClicked()
        {
            controller.ChangeInGameState(InGameState.WorldMap);
        }
    }
}