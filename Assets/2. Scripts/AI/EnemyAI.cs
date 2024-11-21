using UnityEngine;

namespace ObjectAI
{
    public class EnemyAI : ObjectAI
    {
        protected override void EncounterBehavior()
        {
            if (targetEnemy == null)
            {
                fsm = FSM.Idle;
                return;
            }
            
            agent.SetDestination(targetEnemy.position); //적을 향해 돌진

            //공격 범위 체크
            Transform obj = CheckRange(status.attackRange, targetTag);
            if (obj == null)
                return;
            else
                fsm = FSM.Attack;
        }

        protected override void IdleBehavior()
        {
            //화물로 이동
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
