using Scripts.Controller;
using Scripts.Interface;
using Scripts.UI;
using Scripts.Utils;
using UnityEngine.SceneManagement;

namespace Scripts.InGame.State
{
    public class StageOverState : IInGameState
    {
        private readonly InGameSceneController controller;
        private readonly GameUI gameUI;

        public StageOverState(InGameSceneController controller, GameUI gameUI)
        {
            this.controller = controller;
            this.gameUI = gameUI;
        }

        public void Enter()
        {
            // UI 초기화
            gameUI.ShowGameOverUI();
            
            // 이벤트 구독
            gameUI.OnRetryClick += HandleRetry;
            gameUI.OnMainMenuClick += HandleMainMenu;
        }

        public void Update()
        {
            // 게임오버 상태에서는 특별한 업데이트가 필요 없음
        }

        public void Exit()
        {
            // UI 정리
            gameUI.HideGameOverUI();
            
            // 이벤트 구독 해제
            gameUI.OnRetryClick -= HandleRetry;
            gameUI.OnMainMenuClick -= HandleMainMenu;
        }

        private void HandleRetry()
        {
            // 다시 유닛 배치 상태로 돌아감
            controller.ChangeInGameState(InGameState.UnitPlacement);
        }

        private void HandleMainMenu()
        {
            SceneManager.LoadScene("Title");
        }
    }
}
