using Scripts.Controller;
using Scripts.InGame.System;
using Scripts.Interface;
using Scripts.UI;
using Scripts.UI.GameUISub;
using Scripts.Utils;
using UnityEngine;

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
            stageSystem.LoadStage();
            
            preCombatUI.OnStageButtonClicked += HandleStageButtonClicked;
            preCombatUI.OnExitClicked += HandleExitClicked;
            
            preCombatUI.Initialized(stageSystem.GetCurrentStageData());

            // 배치 시스템 초기화 및 유닛 배치
            var cargo = stageSystem.GetCargo();
            if (cargo == null)
            {
                Debug.LogError("Cargo not found");
                return;
            }

            // 카메라를 화물 기준 쿼터뷰 위치로 설정
            if (Camera.main != null)
            {
                Vector3 cargoPosition = cargo.transform.position;
                Vector3 cameraOffset = new Vector3(-30f, 30f, -30f);
                Camera.main.transform.position = cargoPosition + cameraOffset;
                Camera.main.transform.rotation = Quaternion.Euler(35f, 45f, 0);
            }
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