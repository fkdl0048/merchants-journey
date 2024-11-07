using UnityEngine;
using UnityEngine.AI;


public class MoveClick : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent enemyMesh;

    private void Update()
    {
        if(Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray, out RaycastHit hit))
            {
                enemyMesh.SetDestination(hit.point);
            }
        }
    }
}
