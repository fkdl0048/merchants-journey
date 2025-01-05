using Scripts.Controller;
using Scripts.InGame.System;
using Scripts.Interface;
using Scripts.UI;
using Scripts.Utils;
using UnityEngine;
using System.Collections.Generic;
using Scripts.Manager;
using AI;
using System.Linq;

namespace Scripts.InGame.State
{
    public class UnitPlacementState : IInGameState
    {
        private readonly InGameSceneController controller;
        private readonly GameUI gameUI;
        private readonly UnitSystem unitSystem;
        private readonly StageSystem stageSystem;
        
        private GameObject selectedUnit;

        public UnitPlacementState(InGameSceneController controller, GameUI gameUI, UnitSystem unitSystem, StageSystem stageSystem)
        {
            this.controller = controller;
            this.gameUI = gameUI;
            this.unitSystem = unitSystem;
            this.stageSystem = stageSystem;
        }

        public void Enter()
        {
            // UI 초기화
            gameUI.ShowUnitPlacementUI();
            
            // 이벤트 구독
            gameUI.OnUnitPlacementComplete += HandlePlacementComplete;
            
            // 스테이지 로드
            //stageSystem.LoadStage();
            
            // 배치 시스템 초기화 및 유닛 배치
            var cargo = stageSystem.GetCargo();
            if (cargo == null)
            {
                Debug.LogError("Cargo not found");
                return;
            }
            
            // 카메라를 화물 기준 쿼터뷰 위치로 설정
            if (Camera.main != null)
            {
                Vector3 cargoPosition = cargo.transform.position;
                Vector3 cameraOffset = new Vector3(-30f, 30f, -30f);
                Camera.main.transform.position = cargoPosition + cameraOffset;
                Camera.main.transform.rotation = Quaternion.Euler(35f, 45f, 0);
            }
            
            unitSystem.Initialize(cargo);
            unitSystem.SpawnInitialUnits();

            //HighlightPlacementArea(cargo);
        }

        public void Exit()
        {
            gameUI.OnUnitPlacementComplete -= HandlePlacementComplete;
            selectedUnit = null;
        }

        public void Update()
        {
            if (Input.GetMouseButtonDown(0)) // Left click
            {
                HandleLeftClick();
            }
            else if (Input.GetMouseButtonDown(1)) // Right click
            {
                HandleRightClick();
            }
        }

        private void HandleLeftClick()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hitUnit = Physics.RaycastAll(ray)
                .Where(x => x.collider.CompareTag("Unit")).ToArray();
            if (hitUnit.Length == 0)
                return;
            //광선에 맞은 첫번째 unit을 색적
            selectedUnit = hitUnit[0].collider.gameObject;
            Debug.Log(selectedUnit.name);
        }

        private void HandleRightClick()
        {
            // 선택된 유닛이 없을 경우에
            if (selectedUnit == null)
                return;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //만약 선택한 곳에 Cargo가 있을 경우에 return
            bool isHitCargo = Physics.RaycastAll(ray)
                .Where(x => x.collider.CompareTag("Cargo")).Count() > 0;
            if (isHitCargo)
                return;

            RaycastHit hitTile = Physics.RaycastAll(ray)
                .Where(x => x.collider.CompareTag("Tile")).ToArray()[0];
            //만약 선택한 곳에 타일이 하나도 없을 경우에 return
            if (hitTile.collider == null)
            {
                selectedUnit = null;
                return;
            }

            //이동 성공
            var tile = hitTile.collider.gameObject.GetComponent<Tile>();
            if (unitSystem.MoveUnit(selectedUnit, tile.transform.position, true))
            {
                //타일 hasUnit 이동해줌
                var player = selectedUnit.GetComponent<PlayerAI>();
                player.GetComponent<PlayerAI>().myTile.hasUnit = false;
                player.GetComponent<PlayerAI>().myTile = tile;

                selectedUnit = null; // 선택 해제
            }
        }

        private void HandlePlacementComplete()
        {
            controller.ChangeInGameState(InGameState.Wave);
            EventManager.Instance.TriggerEvent("AIEnableTrigger", null);
        }
    }
}