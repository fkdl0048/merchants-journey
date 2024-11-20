using Scripts.Controller;
using Scripts.InGame.System;
using Scripts.Interface;
using Scripts.UI;
using Scripts.Utils;
using UnityEngine; // Added using UnityEngine for GameObject

namespace Scripts.InGame.State
{
    public class BattleState : IInGameState
    {
        private readonly InGameSceneController controller;
        private readonly GameUI gameUI;
        private readonly StageSystem stageController;
    //   private readonly BattleSystem 유닛과 적을 관리하는;
    
        private Cargo cargo;

        public BattleState(InGameSceneController controller, GameUI gameUI, StageSystem stageController)
        {
            this.controller = controller;
            this.gameUI = gameUI;
            this.stageController = stageController;
        }

        public void Enter()
        {
            // UI 초기화
            gameUI.ShowWaveUI();
            
            // 이벤트 구독
            gameUI.OnUnitPlacementComplete += HandleWaveComplete;
            
            // 모든 Cargo 움직임 시작
            cargo = stageController.GetCargo();
            cargo.StartMoving();
        }

        public void Update()
        {
            // 실제 게임에서는 여기서 웨이브 진행 상황을 업데이트
            // 유닛과 적 업데이트
            //BattleSystem.Update();
            
        
            // UI 업데이트
            //gameUI.UpdateWaveProgress(waveManager.GetWaveProgress());
        }

        public void Exit()
        {
            //gameUI.HideWaveUI();
        
            // 전투 시스템 정리
            //BattleSystem.EndWave();
        
            cargo.StopMoving();
            
            // UI 정리
            gameUI.HideWaveUI();
            
            // 이벤트 구독 해제
            gameUI.OnUnitPlacementComplete -= HandleWaveComplete;
        }

        private void HandleWaveComplete()
        {
            // if (waveManager.IsLastWave())
            // {
            //     controller.ChangeInGameState(InGameState.GameClear);
            // }
            // else
            // {
            //     waveManager.PrepareNextWave();
            //     controller.ChangeInGameState(InGameState.UnitPlacement);
            // }
            
            // 웨이브가 완료되면 게임 클리어 상태로 전환
            controller.ChangeInGameState(InGameState.StageClear);
        }

        private void HandleGameOver()
        {
            //controller.ChangeInGameState(InGameState.GameOver);
        }
    }
}