using Scripts.Controller;
using Scripts.InGame;
using Scripts.InGame.System;
using Scripts.Interface;
using Scripts.UI;
using Scripts.Utils;
using UnityEngine;

namespace Scripts.InGame.State
{
    public class UnitPlacementState : IInGameState
    {
        private readonly InGameSceneController controller;
        private readonly GameUI gameUI;
        private readonly UnitSystem unitSystem;
        private readonly StageSystem stageController;

        public UnitPlacementState(InGameSceneController controller, GameUI gameUI, UnitSystem unitSystem, StageSystem stageController)
        {
            this.controller = controller;
            this.gameUI = gameUI;
            this.unitSystem = unitSystem;
            this.stageController = stageController;
        }

        public void Enter()
        {
            // 스테이지 로드
            stageController.LoadStage();
            // if (stageData == null)
            // {
            //     Debug.LogError("Failed to load stage data!");
            //     return;
            // }
            
            // 배치 시스템 초기화
            //unitSystem.EnablePlacementMode();
            
            // UI 초기화
            gameUI.ShowUnitPlacementUI();
            
            // 이벤트 구독
            gameUI.OnUnitPlacementComplete += HandlePlacementComplete;
        }

        public void Update()
        {
            // 마우스 입력 처리
            if (Input.GetMouseButtonDown(0))
            {
                HandleUnitPlacement();
            }
            
            // 유닛 배치 프리뷰 업데이트
            UpdatePlacementPreview();
        }

        public void Exit()
        {
            
            // 배치 시스템 정리
            //unitSystem.DisablePlacementMode();
            // UI 정리
            gameUI.HideUnitPlacementUI();
            
            // 이벤트 구독 해제
            gameUI.OnUnitPlacementComplete -= HandlePlacementComplete;
        }

        private void HandleUnitPlacement()
        {
            // Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // if (unitSystem.CanPlaceUnitAtPosition(mousePosition))
            // {
            //     unitSystem.PlaceSelectedUnit(mousePosition);
            // }
        }

        private void UpdatePlacementPreview()
        {
            // Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // unitSystem.UpdatePlacementPreview(mousePosition);
        }

        private void HandlePlacementComplete()
        {
            // 유닛 배치가 완료되면 Wave 상태로 전환
            controller.ChangeInGameState(InGameState.Wave);
        }
    }
}