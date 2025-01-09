using System.Collections.Generic;
using System.Linq;
using AI;
using Scripts.Data;
using Scripts.Manager;
using Scripts.Utils;
using Unity.VisualScripting;
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
        [SerializeField] private GameObject[] unitPrefabs;
        [SerializeField] private LayerMask tileLayer;
        
        private Cargo currentCargo;
        private List<GameObject> spawnedUnits = new List<GameObject>();
        private GameObject tileParent;
        private List<Tile> tileList;
        //Init
        public void Initialize(Cargo cargo)
        {
            currentCargo = cargo;
            spawnedUnits.Clear();

            tileParent = currentCargo.GetComponentInChildren<CargoTileGenerator>().gameObject;
            tileList = tileParent.GetComponentsInChildren<Tile>().ToList();
        }

        //유닛 스폰 관련 함수
        public void SpawnInitialUnits()
        {
            //예외 처리.
            if (currentCargo == null || unitPrefabs == null || unitPrefabs.Length == 0 || tileList.Count == 0)
                return;

            GameData gameData = SaveManager.Instance.GetGameData();
            if (gameData == null || gameData.ownedUnits == null)
            {
                Debug.LogError("[UnitSystem] GameData or ownedUnits is null");
                return;
            }

            for(int i = 0; i < gameData.ownedUnits.Count; i++)
            {
                int ind = Random.Range(0, tileList.Count);
                Tile tile = tileList[ind];
                if (tile == null)
                    break;
                if (tile.hasUnit == true)
                    i--;
                else
                {
                    var obj = SpawnUnit(gameData.ownedUnits[i].unitType, tile);
                    if(obj != null)
                        tile.hasUnit = true;
                }
            }
        }
        public GameObject SpawnUnit(UnitType unitType, Tile tile)
        {
            Vector3 tileCenter = tile.transform.position;
            
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
            spawnedUnit.GetComponent<PlayerAI>().Setup(currentCargo, tileCenter, false, tile);
            if (spawnedUnit.TryGetComponent<NavMeshAgent>(out var agent))
            {
                if (NavMesh.SamplePosition(spawnedUnit.transform.position, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
                {
                    agent.Warp(hit.position); // 위치를 NavMesh 위로 조정
                }
                else
                    Debug.LogError("NavMesh 위에서 에이전트를 생성할 수 없습니다.");
            }

            spawnedUnits.Add(spawnedUnit);
            return spawnedUnit;
        }
        public bool MoveUnit(GameObject unit, Vector3 targetPosition, bool immediately)
        {
            if (unit == null || currentCargo == null)
                return false;

            if (immediately)
                unit.transform.position = targetPosition;
            //unit 추적 위치 변경
            var player = unit.GetComponent<PlayerAI>();
            player.ChangeTargetPostion(targetPosition);
            player.ChangeForce(true);

            return true;
        }

        //초기화
        public void ClearUnits()
        {
            foreach (var unit in spawnedUnits)
            {
                if (unit != null)
                    Destroy(unit);
            }
            spawnedUnits.Clear();
        }
        private void OnDestroy()
        {
            ClearUnits();
        }
        public void OnStageChange()
        {
            ClearUnits();
        }

        //유틸 스크립트
        public List<GameObject> GetSpawnUnits() => spawnedUnits;
        public void EnableHighlightTile(bool enable)
        {
            tileParent.SetActive(enable);
        }
    }
}