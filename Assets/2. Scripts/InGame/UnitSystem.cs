using System.Collections.Generic;
using Scripts.Data;
using Scripts.Manager;
using UnityEngine;

namespace Scripts.InGame
{
    public class UnitSystem : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform unitsContainer;
        [SerializeField] private LayerMask tileLayerMask;
        
        private List<UnitData> placedUnits = new List<UnitData>();
        private List<Tile> availableTiles = new List<Tile>();
        private UnitData selectedUnit;
        private bool isPlacementMode;

        private Camera mainCamera;
        
        private void Awake()
        {
            mainCamera = Camera.main;
        }

        public void EnablePlacementMode()
        {
            isPlacementMode = true;
            LoadStageData();
            SetupAvailableTiles();
            PlaceInitialUnits();
        }

        public void DisablePlacementMode()
        {
            isPlacementMode = false;
            ClearAvailableTiles();
        }

        private void LoadStageData()
        {
            // SaveData에서 현재 스테이지 정보 로드
            currentStageData = 1 //SaveManager.Instance.GetCurrentStageData();
        }

        private void SetupAvailableTiles()
        {
            availableTiles.Clear();

            // 화물의 위치와 사거리를 기준으로 타일 찾기
            foreach (var cargo in currentStageData.cargoData)
            {
                // 화물의 배치 가능 범위 내의 타일들 찾기
                Collider[] colliders = Physics.OverlapSphere(cargo.position, cargo.placementRadius, tileLayerMask);
                
                foreach (var collider in colliders)
                {
                    Tile tile = collider.GetComponent<Tile>();
                    if (tile != null && tile.isWalkable && !availableTiles.Contains(tile))
                    {
                        availableTiles.Add(tile);
                        // 배치 가능한 타일 시각적 표시
                        HighlightTile(tile, true);
                    }
                }
            }
        }

        private void PlaceInitialUnits()
        {
            // 현재 보유 중인 유닛 정보 가져오기
            List<UnitData> availableUnits = SaveDataManager.Instance.GetAvailableUnits();
            
            foreach (var unitData in availableUnits)
            {
                // 랜덤한 위치에 유닛 배치
                PlaceUnitRandomly(unitData);
            }
        }

        private void PlaceUnitRandomly(UnitData unitData)
        {
            if (availableTiles.Count == 0) return;

            int randomIndex = Random.Range(0, availableTiles.Count);
            Tile randomTile = availableTiles[randomIndex];

            // 유닛 생성 및 배치
            CreateUnit(unitData, randomTile.transform.position);
            
            // 해당 타일 사용 불가 처리
            availableTiles.RemoveAt(randomIndex);
        }

        private Unit CreateUnit(UnitData unitData, Vector3 position)
        {
            GameObject unitObj = Instantiate(unitData.prefab, position, Quaternion.identity, unitsContainer);
            Unit unit = unitObj.GetComponent<Unit>();
            
            if (unit != null)
            {
                unit.Initialize(unitData);
                placedUnits.Add(unit);
            }
            
            return unit;
        }

        public void HandleUnitSelection(Vector3 mousePosition)
        {
            Ray ray = mainCamera.ScreenPointToRay(mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // 유닛 선택 처리
                Unit clickedUnit = hit.collider.GetComponent<Unit>();
                if (clickedUnit != null && placedUnits.Contains(clickedUnit))
                {
                    SelectUnit(clickedUnit);
                }
                // 타일 선택 처리 (유닛 이동용)
                else
                {
                    Tile clickedTile = hit.collider.GetComponent<Tile>();
                    if (clickedTile != null && selectedUnit != null && availableTiles.Contains(clickedTile))
                    {
                        MoveSelectedUnit(clickedTile.transform.position);
                    }
                }
            }
        }

        private void SelectUnit(Unit unit)
        {
            if (selectedUnit != null)
            {
                // 이전 선택 유닛 하이라이트 제거
                selectedUnit.SetSelected(false);
            }

            selectedUnit = unit;
            selectedUnit.SetSelected(true);
        }

        private void MoveSelectedUnit(Vector3 position)
        {
            if (selectedUnit != null)
            {
                selectedUnit.transform.position = position;
                selectedUnit.SetSelected(false);
                selectedUnit = null;
            }
        }

        private void HighlightTile(Tile tile, bool highlight)
        {
            // 타일 하이라이트 처리 (머테리얼 변경 등)
            tile.SetHighlight(highlight);
        }

        private void ClearAvailableTiles()
        {
            foreach (var tile in availableTiles)
            {
                HighlightTile(tile, false);
            }
            availableTiles.Clear();
        }

        public List<Vector3> GetUnitPositions()
        {
            List<Vector3> positions = new List<Vector3>();
            foreach (var unit in placedUnits)
            {
                positions.Add(unit.transform.position);
            }
            return positions;
        }

        public void ValidateUnitPlacement()
        {
            // 유닛 배치 검증 로직
            // 예: 모든 화물 근처에 최소 1개 이상의 유닛이 있는지 확인
            bool isValid = true;
            
            foreach (var cargo in currentStageData.cargoData)
            {
                bool hasNearbyUnit = false;
                foreach (var unit in placedUnits)
                {
                    if (Vector3.Distance(cargo.position, unit.transform.position) <= cargo.placementRadius)
                    {
                        hasNearbyUnit = true;
                        break;
                    }
                }
                
                if (!hasNearbyUnit)
                {
                    isValid = false;
                    break;
                }
            }

            return isValid;
        }
    }
}