using AI;
using Scripts.Controller;
using Scripts.InGame.System;
using Scripts.Interface;
using Scripts.UI;
using Scripts.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.InGame.State
{
    public class BattleState : IInGameState
    {
        private readonly InGameSceneController controller;
        private readonly GameUI gameUI;
        private readonly UnitSystem unitSystem;
        private readonly StageSystem stageController;
        private readonly ClickSystem clickSystem;
    //  private readonly BattleSystem 유닛과 적을 관리하는 클래스 추가되어야 할듯
    
        private Cargo cargo;
        private Color originalTileColor = Color.white;
        private Color highlightColor = Color.red;
        private List<MeshRenderer> highlightedTiles;
        private GameObject selectedUnit;

        public BattleState(InGameSceneController controller, GameUI gameUI, UnitSystem unitSystem, StageSystem stageController, ClickSystem clickSystem)
        {
            this.controller = controller;
            this.gameUI = gameUI;
            this.unitSystem = unitSystem;
            this.stageController = stageController;
            this.clickSystem = clickSystem;
        }

        public void Enter()
        {
            highlightedTiles = new List<MeshRenderer>();

            gameUI.ShowWaveUI();
            
            gameUI.OnUnitPlacementComplete += HandleWaveComplete;

            cargo = stageController.GetCargo();
            cargo.OnDestinationReached += HandleCargoDestinationReached;
            cargo.StartMoving();

            //유닛 AI 작동 (우성)
            UnitAIEnable(unitSystem.GetSpawnUnits().ToArray());
        }
        
        public void Update()
        {   
            // 카메라가 화물을 따라가도록 업데이트
            if (Camera.main != null && cargo != null)
            {
                Vector3 cargoPosition = cargo.transform.position;
                Vector3 cameraOffset = new Vector3(-30f, 30f, -30f);
                Camera.main.transform.position = cargoPosition + cameraOffset;
                Camera.main.transform.rotation = Quaternion.Euler(35f, 45f, 0f);
            }

            // 유닛 이동 로직
            if (Input.GetMouseButtonDown(0)) // Left click
            {
                var obj = clickSystem.GetMouseDownGameobject("Unit");
                selectedUnit = obj;
            }
            else if(Input.GetMouseButtonDown(1))
                HandleRightClick();
            
            // 개발용 키
            if (Input.GetKeyDown(KeyCode.Q))
            {
                HandleCargoDestinationReached();
            }
        }

        public void Exit()
        {        
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

        private void UnitAIEnable(GameObject[] obj)
        {
            foreach (GameObject ai in obj)
                ai.GetComponent<ObjectAI>().aiEnable = true;
        }


        private void HandleRightClick()
        {
            if (selectedUnit == null) 
                return;

            var obj = clickSystem.GetMouseDownGameobject("Tile");
            if (obj == null)
                return;

            unitSystem.MoveUnit(selectedUnit, obj.transform.position, false);
            selectedUnit = null;
        }
    }
}