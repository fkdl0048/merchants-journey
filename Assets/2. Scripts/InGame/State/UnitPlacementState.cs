using Scripts.Controller;
using Scripts.InGame;
using Scripts.Interface;
using Scripts.UI;
using UnityEngine;

namespace Scripts.InGame.State
{
    public class UnitPlacementState : IInGameState
    {
        private readonly InGameSceneController controller;
        private readonly GameUI gameUI;
        private readonly UnitSystem unitSystem;

        public UnitPlacementState(InGameSceneController controller, GameUI gameUI, UnitSystem unitSystem)
        {
            this.controller = controller;
            this.gameUI = gameUI;
            this.unitSystem = unitSystem;
        }

        public void Enter()
        {
            // UI 초기화
            gameUI.ShowUnitPlacementUI();
            
            // 배치 시스템 초기화
            //unitSystem.EnablePlacementMode();
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
            // UI 정리
            gameUI.HideUnitPlacementUI();
            
            // 배치 시스템 정리
            //unitSystem.DisablePlacementMode();
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
    }
}