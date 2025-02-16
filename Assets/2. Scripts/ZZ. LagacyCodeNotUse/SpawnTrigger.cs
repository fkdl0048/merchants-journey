using System.Collections;
using UnityEngine;

public class SpawnTrigger : MonoBehaviour
{
    [Header("EnemySpawner")]
    [SerializeField] private EnemySpawnerBasedTrigger[] spawners;
    [SerializeField] private float termSpawnTime;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cargo") == false)
            return;
        StartCoroutine(SpawnEnemy());
    }

    private IEnumerator SpawnEnemy()
    {
        foreach (var spawner in spawners)
        {
            spawner.Spawn();
            yield return new WaitForSeconds(termSpawnTime);
        }
        yield return null;
    }
    
}
