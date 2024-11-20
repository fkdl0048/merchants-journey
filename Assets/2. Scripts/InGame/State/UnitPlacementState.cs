using Scripts.Controller;
using Scripts.InGame;
using Scripts.InGame.System;
using Scripts.Interface;
using Scripts.UI;
using Scripts.Utils;
using UnityEngine;
using System.Collections.Generic;

namespace Scripts.InGame.State
{
    public class UnitPlacementState : IInGameState
    {
        private readonly InGameSceneController controller;
        private readonly GameUI gameUI;
        private readonly UnitSystem unitSystem;
        private readonly StageSystem stageController;
        
        private List<MeshRenderer> highlightedTiles = new List<MeshRenderer>();
        private Color originalTileColor;
        // 파랑색
        private static readonly Color highlightColor = new Color(0.2f, 0.2f, 1f, 0.6f);

        public UnitPlacementState(InGameSceneController controller, GameUI gameUI, UnitSystem unitSystem, StageSystem stageController)
        {
            this.controller = controller;
            this.gameUI = gameUI;
            this.unitSystem = unitSystem;
            this.stageController = stageController;
        }

        public void Enter()
        {
            // UI 초기화
            gameUI.ShowUnitPlacementUI();
            
            // 이벤트 구독
            gameUI.OnUnitPlacementComplete += HandlePlacementComplete;
            
            // 스테이지 로드
            stageController.LoadStage();
            
            // 배치 시스템 초기화 및 유닛 배치
            var cargo = stageController.GetCargo();
            if (cargo == null)
            {
                return;
            }
            
            unitSystem.Initialize(cargo);
            HighlightPlacementArea(cargo);
            unitSystem.SpawnInitialUnits();
        }

        private void HighlightPlacementArea(Cargo cargo)
        {
            // 이전에 하이라이트된 타일들 원래 색상으로 복원
            ResetTileColors();
            
            Vector3 cargoPosition = cargo.transform.position;
            float searchRadius = unitSystem.PlacementRange * unitSystem.TileSize;
            
            // 카고 주변의 원형 영역 내 타일 검사
            Collider[] colliders = Physics.OverlapSphere(cargoPosition, searchRadius, LayerMask.GetMask("Tile"));
            
            foreach (var collider in colliders)
            {
                var tile = collider.GetComponent<Tile>();
                if (tile != null && tile.isWalkable)
                {
                    Vector2Int tilePos = unitSystem.WorldToTilePosition(collider.transform.position);
                    Vector2Int cargoTilePos = unitSystem.WorldToTilePosition(cargoPosition);
                    
                    // 맨해튼 거리 계산
                    int dx = Mathf.Abs(tilePos.x - cargoTilePos.x);
                    int dy = Mathf.Abs(tilePos.y - cargoTilePos.y);
                    
                    // placementRange 이내의 타일만 하이라이트
                    if (dx + dy <= unitSystem.PlacementRange)
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

        public void Update()
        {
            // 필요한 경우 업데이트 로직 구현
        }

        public void Exit()
        {
            Debug.Log("[UnitPlacementState] Exit");
            
            ResetTileColors();
            // UI 정리
            gameUI.HideUnitPlacementUI();
            
            // 이벤트 구독 해제
            gameUI.OnUnitPlacementComplete -= HandlePlacementComplete;
            
            Debug.Log("[UnitPlacementState] Exit completed");
        }

        private void HandlePlacementComplete()
        {
            Debug.Log("[UnitPlacementState] Placement complete - Changing to Wave state");
            controller.ChangeInGameState(InGameState.Wave);
        }
    }
}