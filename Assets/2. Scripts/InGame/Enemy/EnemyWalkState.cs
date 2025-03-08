using Abstract;
using EnemyScript;
using UnityEngine;

namespace State
{
    public class EnemyWalkState : EnemyState
    {
        EnemyMoveBehaviour enemyMoveBehaviour;
        Transform target;
        float attackRange;

        public EnemyWalkState(EnemyMoveBehaviour enemyMoveBehaviour, Transform target, float attackRange)
        {
            this.enemyMoveBehaviour = enemyMoveBehaviour;
            this.target = target;
            this.attackRange = attackRange;
        }
        public override void Enter()
        {
            return;
        }

        public override EnemySignal Exit()
        {
            return EnemySignal.AttackSignal;
        }

        public override EnemySignal OnAction()
        {
            return EnemySignal.None;
        }

        public override EnemySignal OnActionUpdate()
        {
            PreCheck();
            
            if (enemyMoveBehaviour.IsInRange(target, attackRange))
                return Exit();
            else
                enemyMoveBehaviour.MoveBehaviour(target);

            return EnemySignal.None;
        }
    }
}