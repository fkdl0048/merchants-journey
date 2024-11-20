using System.Collections.Generic;
using Scripts.Data;
using Scripts.Manager;
using Scripts.Utils;
using UnityEngine;

namespace Scripts.InGame.System
{
    public class UnitSystem : MonoBehaviour
    {
        [SerializeField] private float tileSize = 1f;
        [SerializeField] private GameObject[] unitPrefabs;
        [SerializeField] private LayerMask tileLayer;
        
        private Cargo currentCargo;
        private List<GameObject> spawnedUnits = new List<GameObject>();
        private HashSet<Vector2Int> occupiedTiles = new HashSet<Vector2Int>();

        public float TileSize => tileSize;
        public int PlacementRange => currentCargo != null ? currentCargo.placementRange : 0;

        public void Initialize(Cargo cargo)
        {
            currentCargo = cargo;
            occupiedTiles.Clear();
            spawnedUnits.Clear();
        }

        public void SpawnInitialUnits()
        {
            if (currentCargo == null || unitPrefabs == null || unitPrefabs.Length == 0)
                return;

            // 각 유닛 타입별로 최대 3번까지 시도
            TrySpawnUnitWithRetry(UnitType.Pyosa, 3);
            TrySpawnUnitWithRetry(UnitType.Archer, 3);
            TrySpawnUnitWithRetry(UnitType.Warrior, 3);

            // 만약 생성된 유닛이 3개 미만이면 경고
            if (spawnedUnits.Count < 3)
            {
                Debug.LogWarning($"[UnitSystem] Failed to spawn all units. Only spawned {spawnedUnits.Count} units.");
            }
        }

        private void TrySpawnUnitWithRetry(UnitType unitType, int maxAttempts)
        {
            for (int attempt = 0; attempt < maxAttempts; attempt++)
            {
                GameObject spawnedUnit = SpawnUnitRandomly(unitType);
                if (spawnedUnit != null)
                {
                    return; // 성공하면 즉시 반환
                }
                Debug.LogWarning($"[UnitSystem] Failed to spawn {unitType} (Attempt {attempt + 1}/{maxAttempts})");
            }
        }

        public GameObject SpawnUnit(UnitType unitType, Vector3 position)
        {
            Vector2Int tilePos = WorldToTilePosition(position);
            
            if (!IsPositionValid(tilePos))
                return null;

            Vector3 tileCenter = TileToWorldPosition(tilePos);
            
            if ((int)unitType >= unitPrefabs.Length)
                return null;

            GameObject unitPrefab = unitPrefabs[(int)unitType];
            if (unitPrefab == null)
                return null;

            GameObject spawnedUnit = Instantiate(unitPrefab, tileCenter, Quaternion.identity);
            // 유닛을 cargo의 자식으로 설정
            spawnedUnit.transform.SetParent(currentCargo.transform);
            
            occupiedTiles.Add(tilePos);
            spawnedUnits.Add(spawnedUnit);
            return spawnedUnit;
        }

        public GameObject SpawnUnitRandomly(UnitType unitType)
        {
            Vector2Int randomTilePos = GetRandomAvailableTile();
            if (randomTilePos == Vector2Int.zero)
            {
                Debug.LogWarning($"[UnitSystem] Failed to find available tile for {unitType}");
                return null;
            }
                
            Vector3 worldPos = TileToWorldPosition(randomTilePos);
            return SpawnUnit(unitType, worldPos);
        }

        private Vector2Int GetRandomAvailableTile()
        {
            if (currentCargo == null)
                return Vector2Int.zero;

            List<Vector2Int> availableTiles = new List<Vector2Int>();
            Vector3 cargoPosition = currentCargo.transform.position;
            Vector2Int cargoTilePos = WorldToTilePosition(cargoPosition);

            // 카고 주변의 사각형 영역 내의 모든 타일 검사
            for (int x = -currentCargo.placementRange; x <= currentCargo.placementRange; x++)
            {
                for (int y = -currentCargo.placementRange; y <= currentCargo.placementRange; y++)
                {
                    Vector2Int tilePos = new Vector2Int(cargoTilePos.x + x, cargoTilePos.y + y);
                    
                    // 맨해튼 거리가 placementRange 이내인지 확인
                    if (Mathf.Abs(x) + Mathf.Abs(y) <= currentCargo.placementRange)
                    {
                        Vector3 worldPos = TileToWorldPosition(tilePos);
                        
                        // 해당 위치에 타일이 있는지 확인
                        Collider[] colliders = Physics.OverlapSphere(worldPos, 0.1f, tileLayer);
                        if (colliders.Length > 0)
                        {
                            var tile = colliders[0].GetComponent<Tile>();
                            if (tile != null && tile.isWalkable && !occupiedTiles.Contains(tilePos))
                            {
                                availableTiles.Add(tilePos);
                            }
                        }
                    }
                }
            }

            if (availableTiles.Count == 0)
            {
                Debug.LogWarning($"[UnitSystem] No available tiles found around cargo at {cargoPosition}");
                return Vector2Int.zero;
            }

            return availableTiles[Random.Range(0, availableTiles.Count)];
        }

        public Vector2Int WorldToTilePosition(Vector3 worldPos)
        {
            return new Vector2Int(
                Mathf.RoundToInt(worldPos.x / tileSize),
                Mathf.RoundToInt(worldPos.z / tileSize)
            );
        }

        private Vector3 TileToWorldPosition(Vector2Int tilePos)
        {
            return new Vector3(
                tilePos.x * tileSize,
                0,
                tilePos.y * tileSize
            );
        }

        private bool IsPositionValid(Vector2Int tilePos)
        {
            if (currentCargo == null)
                return false;

            if (occupiedTiles.Contains(tilePos))
                return false;

            Vector2Int cargoTilePos = WorldToTilePosition(currentCargo.transform.position);
            int dx = Mathf.Abs(tilePos.x - cargoTilePos.x);
            int dy = Mathf.Abs(tilePos.y - cargoTilePos.y);
            
            if (dx + dy > currentCargo.placementRange)
                return false;

            Vector3 worldPos = TileToWorldPosition(tilePos);
            Collider[] colliders = Physics.OverlapSphere(worldPos, 0.1f, tileLayer);
            if (colliders.Length == 0)
                return false;

            var tile = colliders[0].GetComponent<Tile>();
            return tile != null && tile.isWalkable;
        }

        public void ClearUnits()
        {
            foreach (var unit in spawnedUnits)
            {
                if (unit != null)
                    Destroy(unit);
            }
            spawnedUnits.Clear();
            occupiedTiles.Clear();
        }

        private void OnDestroy()
        {
            ClearUnits();
        }

        // Stage 이동 시 호출될 메서드
        public void OnStageChange()
        {
            ClearUnits();
        }
    }
}