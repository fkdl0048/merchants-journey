using UnityEngine;

public class SpawnTrigger : MonoBehaviour
{
    [Header("EnemySpawner")]
    [SerializeField] private EnemySpawnerBasedTrigger[] spawners;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cargo") == false)
            return;
        foreach (var spawner in spawners)
            spawner.Spawn();
    }
}
