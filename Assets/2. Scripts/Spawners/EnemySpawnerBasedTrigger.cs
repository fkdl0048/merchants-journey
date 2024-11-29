using ObjectAI;
using Scripts.InGame.Stage;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawnerBasedTrigger : EnemySpawner
{
    [Header("Cargo")]
    [SerializeField] private Transform cargo;

    [Header("Spawn Value")]
    [SerializeField] private List<Scripts.Utils.KeyValuePair<GameObject, int>> spawninfos;
    [SerializeField] private bool isSpawn = false;

    public override void Spawn()
    {
        if (isSpawn)
            return;
        isSpawn = true;
        
        foreach(var item in spawninfos)
        {
            GameObject obj = item.value;
            int count = item.value2;

            for (int i = 0; i < count; i++)
                SpawnEnemy(obj);
        }
    }
    public void SpawnEnemy(GameObject enemyObj)
    {
        var obj = Instantiate(enemyObj);
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
    }
}
