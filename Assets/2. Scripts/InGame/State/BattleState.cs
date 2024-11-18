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
        }

        public void Update()
        {
            // 유닛과 적 업데이트
            //BattleSystem.Update();
            
        
            // UI 업데이트
            //gameUI.UpdateWaveProgress(waveManager.GetWaveProgress());
        }

        public void Exit()
        {
            // UI 정리
            //gameUI.HideWaveUI();
        
            // 전투 시스템 정리
            //BattleSystem.EndWave();
        
            // 이벤트 리스너 제거
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
        }

        private void HandleGameOver()
        {
            //controller.ChangeInGameState(InGameState.GameOver);
        }
    }
}