using Scripts.Controller;
using Scripts.Interface;
using Scripts.UI;
using Scripts.Utils;

namespace Scripts.InGame.State
{
    public class BattleState : IInGameState
    {
        private readonly InGameSceneController controller;
        private readonly GameUI gameUI;
    //   private readonly BattleSystem 유닛과 적을 관리하는;

        public BattleState(InGameSceneController controller, GameUI gameUI)
        {
            this.controller = controller;
            this.gameUI = gameUI;
        }

        public void Enter()
        {
            // UI 초기화
            //gameUI.ShowWaveUI();
            //gameUI.UpdateWaveInfo(waveManager.CurrentWave, waveManager.TotalWaves);
        
            // 웨이브 시작
            // BattleSystem 초기화
        
            // 이벤트 리스너 등록
            //BattleSystem.OnWaveComplete += HandleWaveComplete;
            
            // UI 초기화
            gameUI.ShowWaveUI();
            
            // 테스트용: 다음 상태로 전환하는 이벤트 구독
            gameUI.OnUnitPlacementComplete += HandleWaveComplete;
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
        
            // 이벤트 리스너 제거
            
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