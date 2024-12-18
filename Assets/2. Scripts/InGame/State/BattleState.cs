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
            // 실제 게임에서는 여기서 웨이브 진행 상황을 업데이트
            // 유닛과 적 업데이트
            //BattleSystem.Update();
            
            // 카메라가 화물을 따라가도록 업데이트
            if (Camera.main != null && cargo != null)
            {
                Vector3 cargoPosition = cargo.transform.position;
                Vector3 cameraOffset = new Vector3(-30f, 30f, -30f);
                Camera.main.transform.position = cargoPosition + cameraOffset;
                Camera.main.transform.rotation = Quaternion.Euler(35f, 45f, 0f);
            }

            if (Input.GetMouseButtonDown(0)) // Left click
            {
                var obj = clickSystem.GetMouseDownGameobject("Unit");
                selectedUnit = obj;
            }
            else if(Input.GetMouseButtonDown(1))
                HandleRightClick();
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

            unitSystem.MoveUnit(selectedUnit, obj.transform.position);

            selectedUnit = null;
        }

        private void HighlightPlacementArea(Cargo cargo)
        {
            // 이전에 하이라이트된 타일들 원래 색상으로 복원
            ResetTileColors();

            Vector3 cargoPosition = cargo.transform.position;
            Vector2Int cargoTilePos = unitSystem.WorldToTilePosition(cargoPosition);

            // 그리드 시작점 계산
            int startX = cargoTilePos.x - (cargo.width / 2);
            int startY = cargoTilePos.y - (cargo.height / 2);

            // N*M 그리드 영역 내의 타일 검사
            for (int x = 0; x < cargo.width; x++)
            {
                for (int y = 0; y < cargo.height; y++)
                {
                    Vector2Int tilePos = new Vector2Int(startX + x, startY + y);
                    Vector3 worldPos = unitSystem.TileToWorldPosition(tilePos);

                    // 타일 검사
                    Collider[] colliders = Physics.OverlapSphere(worldPos, 0.1f, LayerMask.GetMask("Tile"));
                    foreach (var collider in colliders)
                    {
                        var tile = collider.GetComponent<Tile>();
                        if (tile != null && tile.isWalkable)
                        {
                            var meshRenderer = collider.GetComponent<MeshRenderer>();
                            if (meshRenderer != null)
                            {
                                if (highlightedTiles.Count == 0)
                                {
                                    // 첫 번째 타일의 원래 색상 저장
                                    originalTileColor = meshRenderer.material.color;
                                }
                                meshRenderer.material.color = highlightColor;
                                highlightedTiles.Add(meshRenderer);
                            }
                        }
                    }
                }
            }
        }

        private void ResetTileColors()
        {
            foreach (var meshRenderer in highlightedTiles)
            {
                if (meshRenderer != null)
                {
                    meshRenderer.material.color = originalTileColor;
                }
            }
            highlightedTiles.Clear();
        }
    }
}