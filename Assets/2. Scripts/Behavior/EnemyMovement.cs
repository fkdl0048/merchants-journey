using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [Header("Target Object")]
    [SerializeField] public Transform target;
    [SerializeField] private float speed = 2f;
    private NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    private void Update() 
    {
        agent.SetDestination(target.position);
        //Vector3 dist = transform.position - target.position;
        //float angle = Mathf.Atan2(dist.y, dist.x) * Mathf.Rad2Deg;
        //transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 90));

        //transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
    }

    //Test Code
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
