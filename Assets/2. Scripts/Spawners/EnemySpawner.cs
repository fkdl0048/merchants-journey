using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Value")]
    [SerializeField] private Vector2 spawnTimeRange;
    [SerializeField] private GameObject[] enemyObjects;
    [SerializeField] private bool isStop = false;

    private void Start()
    {
        StartCoroutine(SpawnEnemy());
    }
    private IEnumerator SpawnEnemy()
    {
        if (isStop)
            yield return null;

        //random Enemy Pickup
        int index = Random.Range(0, enemyObjects.Length);
        var obj = Instantiate(enemyObjects[index]);
        obj.transform.parent = transform;
        yield return new WaitForSeconds(Random.Range(spawnTimeRange.x, spawnTimeRange.y));

        StartCoroutine(SpawnEnemy());
    }
}
