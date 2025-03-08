using Abstract;
using EnemyScript;
using UnityEngine;

namespace State
{
    public class EnemyAttackState : EnemyState
    {
        EnemyAttackBehaviour attackBehaviour;
        Transform target;

        public EnemyAttackState(EnemyAttackBehaviour attackBehaviour, Transform target)
        {
            this.attackBehaviour = attackBehaviour;
            this.target = target;
        }
        public override void Enter()
        {
            return;
        }

        public override EnemySignal Exit()
        {
            return EnemySignal.IdleSignal;
        }

        public override EnemySignal OnAction()
        {
            attackBehaviour.Attack();
            return Exit();
        }

        public override EnemySignal OnActionUpdate()
        {
            PreCheck();

            return OnAction();
        }
    }
}