using UnityEngine;

namespace ObjectAI
{
    public class EnemyAI : ObjectAI
    {
        string targetTag = "Player";
        protected override void EncounterBehavior()
        {
            if (targetEnemy == null)
                fsm = FSM.Idle;
            agent.SetDestination(targetEnemy.position);
        }

        protected override void IdleBehavior()
        {
            agent.SetDestination(target.position);

            //탐지 범위 내에 적군 오브젝트가 잡혔다면?
            Transform obj = CheckRange(status.recognizeRange, targetTag);
            if (obj == null)
                return;
            else
            {
                targetEnemy = obj;
                fsm = FSM.Encounter;
            }
        }
    }
}
