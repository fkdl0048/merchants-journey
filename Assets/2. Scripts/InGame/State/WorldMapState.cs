using System;
using Scripts.Controller;
using Scripts.Interface;
using Scripts.UI;
using Scripts.Utils;

namespace Scripts.InGame.State
{
    public class WorldMapState : IInGameState
    {
        private readonly InGameSceneController controller;
        private readonly GameUI gameUI;
        private readonly WorldMapUI worldMapUI;
        
        public WorldMapState(InGameSceneController controller, GameUI gameUI)
        {
            this.controller = controller;
            this.gameUI = gameUI;
            worldMapUI = gameUI.GetWorldMapUI();
        }
        
        public void Enter()
        {
            gameUI.ShowWorldMapUI();
            
            worldMapUI.OnNextStageButtonClicked += HandleNextStageButtonClicked;
        }

        public void Update()
        {
            
        }

        public void Exit()
        {
            
        }
        
        private void HandleNextStageButtonClicked()
        {
            controller.ChangeInGameState(InGameState.UnitPlacement);
        }
    }
}