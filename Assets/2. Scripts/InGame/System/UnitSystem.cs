using System.Collections.Generic;
using Scripts.Utils;
using UnityEngine;


// 11-21 현재 코드는 임시로 랜덤 위치에 유닛 생성
// 이후에 업그레이드? 시스템으로 변경 예정
namespace Scripts.InGame.System
{
    /// <summary>
    /// 유닛을 관리하는 시스템
    /// 각 State에 InGameSceneController를 넘겨주는 방식으로 작동 Dependency Injection
    /// </summary>
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

            // 각 유닛 타입별로 최대 3번까지 시도 (버그 때문에 추가)
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
            
            for (int x = -currentCargo.placementRange; x <= currentCargo.placementRange; x++)
            {
                for (int y = -currentCargo.placementRange; y <= currentCargo.placementRange; y++)
                {
                    Vector2Int tilePos = new Vector2Int(cargoTilePos.x + x, cargoTilePos.y + y);
                    
                    if (Mathf.Abs(x) + Mathf.Abs(y) <= currentCargo.placementRange)
                    {
                        Vector3 worldPos = TileToWorldPosition(tilePos);
  
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
        
        public void OnStageChange()
        {
            ClearUnits();
        }

        public bool MoveUnit(GameObject unit, Vector3 targetPosition)
        {
            if (unit == null || currentCargo == null)
                return false;

            Vector2Int targetTilePos = WorldToTilePosition(targetPosition);
            if (!IsPositionValid(targetTilePos))
                return false;
            
            Vector2Int currentTilePos = WorldToTilePosition(unit.transform.position);
            occupiedTiles.Remove(currentTilePos);
            
            Vector3 newWorldPos = TileToWorldPosition(targetTilePos);
            unit.transform.position = newWorldPos;
            occupiedTiles.Add(targetTilePos);

            return true;
        }
    }
}