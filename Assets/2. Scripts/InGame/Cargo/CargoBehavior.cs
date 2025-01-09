using AI;
using UnityEngine;


public class CargoBehavior : MonoBehaviour
{
    private Cargo cargo;
    public bool isStart = false; //배치가 완료되어야 시작함.
    [SerializeField] GameObject SpawnerGroup;

    private void Awake()
    {
        cargo = GetComponent<Cargo>();
    }
    private void Update()
    {
        if (!isStart)
            return;

        if(GetEnemyCount() > 0)
        {
            cargo.StopMoving();   
        }
        else
        {
            cargo.RestartMoving();
        }
    }
    private int GetEnemyCount() => SpawnerGroup.GetComponentsInChildren<EnemyAI>().Length;

}