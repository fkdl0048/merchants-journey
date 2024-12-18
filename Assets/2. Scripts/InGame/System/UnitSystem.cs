using System.Collections.Generic;
using AI;
using Scripts.Utils;
using UnityEngine;
using UnityEngine.AI;

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

            // 11-23 by 우성
            // 만든 AI 아군 프리팹 생성
            GameObject spawnedUnit = Instantiate(unitPrefab, tileCenter, Quaternion.identity);
            spawnedUnit.transform.SetParent(currentCargo.transform);
            spawnedUnit.transform.eulerAngles = new Vector3(35, 45, 0);
            spawnedUnit.GetComponent<PlayerAI>().Setup(currentCargo, tileCenter, false);
            if (spawnedUnit.TryGetComponent<NavMeshAgent>(out var agent))
            {
                if (NavMesh.SamplePosition(spawnedUnit.transform.position, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
                {
                    agent.Warp(hit.position); // 위치를 NavMesh 위로 조정
                }
                else
                    Debug.LogError("NavMesh 위에서 에이전트를 생성할 수 없습니다.");
            }

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

            Vector2Int cargoTilePos = WorldToTilePosition(currentCargo.transform.position);
            int maxAttempts = 100;  // 무한 루프 방지
            
            // Calculate grid boundaries
            int startX = cargoTilePos.x - (currentCargo.width / 2);
            int startY = cargoTilePos.y - (currentCargo.height / 2);
            
            for (int attempt = 0; attempt < maxAttempts; attempt++)
            {
                int x = startX + Random.Range(0, currentCargo.width);
                int y = startY + Random.Range(0, currentCargo.height);
                Vector2Int tilePos = new Vector2Int(x, y);
                
                if (IsPositionValid(tilePos) && !occupiedTiles.Contains(tilePos))
                    return tilePos;
            }
            
            return Vector2Int.zero;
        }

        private const float TILE_SIZE = 1f;  // 타일의 실제 크기

        public Vector2Int WorldToTilePosition(Vector3 worldPosition)
        {
            return new Vector2Int(
                Mathf.RoundToInt(worldPosition.x / TILE_SIZE),
                Mathf.RoundToInt(worldPosition.z / TILE_SIZE)
            );
        }

        public Vector3 TileToWorldPosition(Vector2Int tilePosition)
        {
            return new Vector3(
                tilePosition.x * TILE_SIZE,
                0,
                tilePosition.y * TILE_SIZE
            );
        }

        private bool IsPositionValid(Vector2Int tilePos)
        {
            if (currentCargo == null)
                return false;

            Vector2Int cargoTilePos = WorldToTilePosition(currentCargo.transform.position);
            
            // Calculate grid boundaries
            int startX = cargoTilePos.x - (currentCargo.width / 2);
            int startY = cargoTilePos.y - (currentCargo.height / 2);
            
            // Check if the position is within the N*M grid
            if (tilePos.x < startX || tilePos.x >= startX + currentCargo.width || 
                tilePos.y < startY || tilePos.y >= startY + currentCargo.height)
                return false;

            Vector3 worldPos = TileToWorldPosition(tilePos);
            Collider[] colliders = Physics.OverlapSphere(worldPos, 0.1f, tileLayer);
            if (colliders.Length == 0)
                return false;

            var tile = colliders[0].GetComponent<Tile>();
            return tile != null && tile.isWalkable && !occupiedTiles.Contains(tilePos);
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
        public List<GameObject> GetSpawnUnits() => spawnedUnits;
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

            //unit 추적 위치 변경
            unit.GetComponent<ObjectAI>().ChangeTargetPostion(newWorldPos);
            occupiedTiles.Add(targetTilePos);

            return true;
        }
    }
}