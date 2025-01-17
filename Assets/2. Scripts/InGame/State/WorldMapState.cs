using System;
using Scripts.Controller;
using Scripts.InGame.System;
using Scripts.Interface;
using Scripts.Manager;
using Scripts.UI;
using Scripts.Utils;

namespace Scripts.InGame.State
{
    public class WorldMapState : IInGameState
    {
        private readonly InGameSceneController controller;
        private readonly StageSystem stageSystem;
        private readonly GameUI gameUI;
        private readonly WorldMapUI worldMapUI;
        
        public WorldMapState(InGameSceneController controller, GameUI gameUI, StageSystem stageSystem)
        {
            this.controller = controller;
            this.gameUI = gameUI;
            worldMapUI = gameUI.GetWorldMapUI();
            this.stageSystem = stageSystem;
        }
        
        public void Enter()
        {
            gameUI.ShowWorldMapUI();
            
            worldMapUI.Initialized();

            worldMapUI.OnNextStageButtonClicked += HandleNextStageButtonClicked;
            worldMapUI.OnUpgradeButtonClicked += HandleUpgradeButtonClicked;
            
            AudioManager.Instance.StopBGM();
        }

        public void Update()
        {
            
        }

        public void Exit()
        {
            worldMapUI.OnNextStageButtonClicked -= HandleNextStageButtonClicked;
            worldMapUI.OnUpgradeButtonClicked -= HandleUpgradeButtonClicked;
        }
        
        private void HandleNextStageButtonClicked(int index)
        {
            stageSystem.CurrentStageNumber = index + 1;
            controller.ChangeInGameState(InGameState.PreCombat);
            //LoadingSceneController.LoadScene();
        }
        
        private void HandleUpgradeButtonClicked()
        {
            controller.ChangeInGameState(InGameState.Upgrade);
        }
    }
}