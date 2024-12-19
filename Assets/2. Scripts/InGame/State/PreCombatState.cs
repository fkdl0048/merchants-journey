using Scripts.Controller;
using Scripts.InGame.System;
using Scripts.Interface;
using Scripts.UI;
using Scripts.UI.GameUISub;
using Scripts.Utils;

namespace Scripts.InGame.State
{
    public class PreCombatState : IInGameState
    {
        private readonly InGameSceneController controller;
        private readonly GameUI gameUI;
        private readonly StageSystem stageSystem;
        private readonly PreCombatUI preCombatUI;
        
        public PreCombatState(InGameSceneController controller, StageSystem stageSystem, GameUI gameUI)
        {
            this.gameUI = gameUI;
            this.controller = controller;
            this.stageSystem = stageSystem;
            preCombatUI = gameUI.GetPreCombatUI();
        }

        public void Enter()
        {
            gameUI.ShowPreCombatUI();
            
            preCombatUI.OnStageButtonClicked += HandleStageButtonClicked;
            preCombatUI.OnExitClicked += HandleExitClicked;
            
            preCombatUI.Initialized();
            
            stageSystem.LoadStage();
        }

        public void Update()
        {
            
        }

        public void Exit()
        {
            preCombatUI.OnStageButtonClicked -= HandleStageButtonClicked;
            preCombatUI.OnExitClicked -= HandleExitClicked;
        }
        
        private void HandleStageButtonClicked()
        {
            controller.ChangeInGameState(InGameState.UnitPlacement);
        }
        
        private void HandleExitClicked()
        {
            controller.ChangeInGameState(InGameState.WorldMap);
        }
    }
}