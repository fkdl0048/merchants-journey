using Scripts.Controller;
using Scripts.InGame.System;
using Scripts.Interface;
using Scripts.UI;
using Scripts.Utils;

namespace Scripts.InGame.State
{
    public class BattleState : IInGameState
    {
        private readonly InGameSceneController controller;
        private readonly GameUI gameUI;
        private readonly StageSystem stageController;
    //  private readonly BattleSystem 유닛과 적을 관리하는 클래스 추가되어야 할듯
    
        private Cargo cargo;

        public BattleState(InGameSceneController controller, GameUI gameUI, StageSystem stageController)
        {
            this.controller = controller;
            this.gameUI = gameUI;
            this.stageController = stageController;
        }

        public void Enter()
        {
            gameUI.ShowWaveUI();
            
            gameUI.OnUnitPlacementComplete += HandleWaveComplete;

            cargo = stageController.GetCargo();
            cargo.OnDestinationReached += HandleCargoDestinationReached;
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
            // 전투 시스템 정리
            //BattleSystem.EndWave();
        
            cargo.StopMoving();
            cargo.OnDestinationReached -= HandleCargoDestinationReached;
            
            // UI 정리
            gameUI.HideWaveUI();
            
            // 이벤트 구독 해제
            gameUI.OnUnitPlacementComplete -= HandleWaveComplete;
        }

        private void HandleWaveComplete()
        {
            // UI처리 때문에 빼긴 했는데 추가 기획보고 작업 예정
        }

        private void HandleCargoDestinationReached()
        {
            controller.ChangeInGameState(InGameState.StageClear);
        }
    }
}