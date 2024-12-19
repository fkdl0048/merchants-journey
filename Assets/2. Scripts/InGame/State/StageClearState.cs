using Scripts.Controller;
using Scripts.InGame.System;
using Scripts.Interface;
using Scripts.Manager;
using Scripts.UI;
using Scripts.Utils;
using UnityEngine.SceneManagement;

namespace Scripts.InGame.State
{
    public class StageClearState : IInGameState
    {
        private readonly InGameSceneController controller;
        private readonly GameUI gameUI;
        private readonly StageSystem stageSystem;

        public StageClearState(InGameSceneController controller, GameUI gameUI, StageSystem stageSystem)
        {
            this.controller = controller;
            this.gameUI = gameUI;
            this.stageSystem = stageSystem;
        }

        public void Enter()
        {
            // UI 표시
            gameUI.ShowGameClearUI();
            
            // 이벤트 리스너 등록
            gameUI.OnNextStageClick += HandleNextStage;
            gameUI.OnMainMenuClick += HandleMainMenu;
        }

        public void Update()
        {
            // 스테이지 클리어 상태에서는 특별한 업데이트가 필요 없음
            // 이후 작업
        }

        public void Exit()
        {
            // UI 정리
            gameUI.HideGameClearUI();
        
            // 이벤트 리스너 제거
            gameUI.OnNextStageClick -= HandleNextStage;
            gameUI.OnMainMenuClick -= HandleMainMenu;
            
            stageSystem.ClearStage();
        }

        private void HandleNextStage()
        {
            // 다음 스테이지를 위해 다시 유닛 배치 상태로
            var gameData = SaveManager.Instance.GetGameData();

            if (gameData.currentStage <= stageSystem.CurrentStageNumber)
            {
                gameData.currentStage++;
            }
            
            SaveManager.Instance.SaveGameData(gameData);
            
            controller.ChangeInGameState(InGameState.WorldMap);
        }

        private void HandleMainMenu()
        {
            // 메인 메뉴로 돌아가기
            SceneManager.LoadScene("Title");
        }
    }
}