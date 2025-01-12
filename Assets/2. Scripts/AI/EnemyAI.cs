using System.Collections;
using UnityEngine;

namespace AI
{
    public class EnemyAI : ObjectAI
    {
        private const string cargoTag = "Cargo";
        protected override void EncounterBehavior()
        {
            if (targetEnemy == null)
            {
                fsm = FSM.Idle;
                return;
            }

            //공격 범위 체크
            Transform obj = CheckRange(status.attackRange, targetTag, cargoTag);
            if (obj == null)
            {
                agent.isStopped = false;
                agent.SetDestination(targetEnemy.position); // 적을 향해 돌진
            }
            else
            {
                fsm = FSM.Attack;
                agent.isStopped = true;
            }
        }
        protected override void IdleBehavior()
        {
            //ChangeTargetPostion(cargo.transform.position);
            agent.isStopped = false;
            agent.SetDestination(cargo.transform.position);
            //탐지 범위 내에 적군 오브젝트가 잡혔다면?
            Transform obj = CheckRange(status.recognizeRange, targetTag, cargoTag);
            if (obj == null)
                return;
            else
            {
                targetEnemy = obj;
                fsm = FSM.Encounter;
            }
        }
        protected override void AttackBehavior()
        {
            StartCoroutine(AttackStart());
        }

        new IEnumerator AttackStart()
        {
            //공격시간 대기
            isAttack = true;
            yield return new WaitForSeconds(status.attackSpeed);
            //공격 사운드 재생
            //AudioManager.Instance.PlaySFX(hitSoundClip);
            //박스 생성
            Collider[] targets = CheckAttackCollider(status.hitboxRange, targetTag, cargoTag);
            //선처리, 공격 대상이 cargo인지?
            if (targets != null)
            {
                if(targets.Length == 1 && targets[0].TryGetComponent<Cargo>(out var cargo))
                {
                    cargo.hp -= 1;
                    if(cargo.hp <= 0)
                    {
                        Debug.Log("Game Over!");
                    }
                }
                else
                {
                    foreach (Collider target in targets)
                    {
                        if (!target.TryGetComponent<ObjectAI>(out var obj))
                            continue;
                        obj.Hitted(status.damage, transform.position);
                    }
                }
            }
            isAttack = false;
            fsm = FSM.Encounter;
        }
    }
}
