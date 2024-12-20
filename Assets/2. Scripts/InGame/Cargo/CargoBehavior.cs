using AI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Rendering;
using UnityEngine;


public class CargoBehavior : MonoBehaviour
{
    [SerializeField] Cargo cargo; 
    [SerializeField] Vector3 boxSize;
    [SerializeField] Vector3 innerBoxSize;

    [SerializeField] List<PlayerAI> playerList;

    private void Start()
    {
        StartCoroutine(EnemyDetect());
        StartCoroutine(FarDistObjectCall());
        
        playerList = transform.parent.GetComponentsInChildren<PlayerAI>().ToList();
    }
    private IEnumerator EnemyDetect()
    {
        Bounds bounds = new Bounds(transform.position, boxSize);
        Bounds innerBounds = new Bounds(transform.position, innerBoxSize);
        int type = 0;
        Collider[] objectsInScene = FindObjectsOfType<Collider>().Where(x => x.CompareTag("Enemy")).ToArray();
        foreach (Collider obj in objectsInScene)
        {
            // 오브젝트의 위치가 Bounds 내에 있는지 확인
            if (bounds.Contains(obj.transform.position))
                type = 1;
            else if (innerBounds.Contains(obj.transform.position))
                type = 2;
        }

        if (type == 1)
        {
            //40% 감속
            cargo.moveSpeed = cargo.OriginMoveSpeed * 0.6f;
        }
        else if (type == 2)
        {
            //70% 감속
            cargo.moveSpeed = cargo.OriginMoveSpeed * 0.3f;
        }
        else
            cargo.moveSpeed = cargo.OriginMoveSpeed;

        yield return new WaitForSeconds(0.5f);

        StartCoroutine(EnemyDetect());
    }
    private IEnumerator FarDistObjectCall()
    {
        Bounds bounds = new Bounds(transform.position, boxSize);
        foreach (PlayerAI obj in playerList)
        {
            if (obj == null)
                continue;

            if (!bounds.Contains(obj.transform.position))
            {
                obj.ChangeForce(true);
            }
        }

        yield return new WaitForSeconds(0.5f);

        StartCoroutine(FarDistObjectCall());
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, boxSize);
        Gizmos.DrawWireCube(transform.position, innerBoxSize);
    }
}