using ObjectAI;
using Scripts.InGame.Stage;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    [Header("Cargo")]
    [SerializeField] private Transform cargo;

    [Header("Spawn Value")]
    [SerializeField] private Vector2 spawnTimeRange;
    [SerializeField] private GameObject[] enemyObjects;
    private bool isEnable = false;

    private void Start()
    {
        StartCoroutine(SpawnEnemy());
    }
    private void Update()
    {
        bool enable = transform.parent.GetComponent<Stage>().aiEnable;
        isEnable = enable;
    }
    private IEnumerator SpawnEnemy()
    {
        if (isEnable == false)
            yield return null;

        //random Enemy Pickup 임시코드
        int index = Random.Range(0, enemyObjects.Length);
        var obj = Instantiate(enemyObjects[index]);
        obj.transform.parent = transform;
        obj.transform.localPosition = new Vector3(0, 0, 0);
        obj.transform.eulerAngles = new Vector3(35, 45, 0);
        obj.GetComponent<EnemyAI>().Setup(transform.parent.GetComponent<Stage>(), cargo);
        if (obj.TryGetComponent<NavMeshAgent>(out var agent))
        {
            if (NavMesh.SamplePosition(obj.transform.position, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
                agent.Warp(hit.position); // 위치를 NavMesh 위로 조정
            else
                Debug.LogError("NavMesh 위에서 에이전트를 생성할 수 없습니다.");
        }

        yield return new WaitForSeconds(Random.Range(spawnTimeRange.x, spawnTimeRange.y));

        StartCoroutine(SpawnEnemy());
    }
}
