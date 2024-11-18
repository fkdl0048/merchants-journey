using Scripts.Controller;
using Scripts.Interface;
using Scripts.Manager;
using Scripts.UI;

namespace Scripts.InGame.State
{
    public class StageClearState : IInGameState
    {
        private readonly InGameSceneController controller;
        private readonly GameUI gameUI;
        //private readonly RewardManager rewardManager; => 보상 관리자 System

        public StageClearState(InGameSceneController controller, GameUI gameUI)
        {
            this.controller = controller;
            this.gameUI = gameUI;
            //this.rewardManager = rewardManager;
        }

        public void Enter()
        {
            // 보상 계산
            //var rewards = rewardManager.CalculateRewards();
        
            // UI 표시
            //gameUI.ShowGameClearUI(rewards);
            //AudioManager.Instance.PlaySFX("GameClear");
        
            // 이벤트 리스너 등록
            gameUI.OnNextStageClick += HandleNextStage;
            gameUI.OnMainMenuClick += HandleMainMenu;
        }

        public void Update()
        {
            // 필요한 경우 업데이트 로직 구현
        }

        public void Exit()
        {
            // UI 정리
            //gameUI.HideGameClearUI();
        
            // 이벤트 리스너 제거
            gameUI.OnNextStageClick -= HandleNextStage;
            gameUI.OnMainMenuClick -= HandleMainMenu;
        }

        private void HandleNextStage()
        {
           // GameManager.Instance.LoadNextStage();
        }

        private void HandleMainMenu()
        {
           // GameManager.Instance.LoadMainMenu();
        }
    }
}