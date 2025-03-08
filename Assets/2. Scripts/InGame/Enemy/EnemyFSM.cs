using Abstract;
using State;
using Unity.Collections;
using UnityEngine;

namespace EnemyScript
{
    public class EnemyScript : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private float attackRange = 2.5f;
        private EnemyMoveBehaviour moveBehaviour;
        private EnemyAttackBehaviour attackBehaviour;

        EnemyState enemyState;
        private void Start()
        {
            moveBehaviour = GetComponent<EnemyMoveBehaviour>();
            attackBehaviour = GetComponent<EnemyAttackBehaviour>();

            enemyState = new EnemyWalkState(moveBehaviour, target, attackRange);
            
        }


        // Update is called once per frame
        void Update()
        {
            if (enemyState == null)
                return;

            EnemySignal signal = enemyState.OnActionUpdate();

            switch (signal)
            {
                case EnemySignal.None:
                case EnemySignal.IdleSignal:
                    enemyState = new EnemyWalkState(moveBehaviour, target, attackRange);
                    break;
                case EnemySignal.RunSignal:
                    break;
                case EnemySignal.AttackSignal:
                    enemyState = new EnemyAttackState(attackBehaviour, target);
                    break;
                case EnemySignal.AttackEndSignal:
                    //state = new PlayerWalkState(playerMovement, animationController, speed, Runspeed, Dashspeed);
                    break;
                case EnemySignal.Error:
                    break;
            }
        }
    }

}
