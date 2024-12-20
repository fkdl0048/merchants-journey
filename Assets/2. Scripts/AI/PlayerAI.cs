using UnityEngine;
using UnityEngine.UIElements;

namespace AI
{
    public class PlayerAI : ObjectAI
    {
        [SerializeField] private bool isForce = false; //이동 명령이 우선시 됨.
        public void ChangeForce(bool isForce) => this.isForce = isForce;
        protected override void EncounterBehavior()
        {
            if (targetEnemy == null || isForce)
            {
                fsm = FSM.Idle;
                return;
            }

            //공격 범위 체크
            Transform obj = CheckRange(status.attackRange, targetTag);
            if (obj == null)
            {
                agent.isStopped = false;
                agent.SetDestination(targetEnemy.position); //적을 향해 돌진
            }
            else
            {
                fsm = FSM.Attack;
                agent.isStopped = true;
            }
        }
        protected override void IdleBehavior()
        {
            
            agent.isStopped = false;
            agent.SetDestination(targetPosition + cargo.transform.position);
    
            //탐지 범위 내에 적군 오브젝트가 잡혔다면?
            Transform obj = CheckRange(status.recognizeRange, targetTag);
            if (IsArrive())
                isForce = false;
            if (obj == null || isForce)
                return;
            else
            {
                targetEnemy = obj;
                fsm = FSM.Encounter;
            }
        }
        private bool IsArrive()
        {
            var target = cargo.transform.position + targetPosition;
            Vector2 a = CMath.RoundVector2Data(new(gameObject.transform.position.x, gameObject.transform.position.z));
            Vector2 t = CMath.RoundVector2Data(new(target.x, target.z)); 
            return a == t;
        }
    }
}
