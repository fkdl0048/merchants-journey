using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

namespace ObjectAI
{
    public enum FSM
    {
        Idle,       //화물을 호위하는, 또는 화물에 접근하고 있는 상태
        Encounter,  //적을 맞닥뜨린 상태
        Attack,     //공격 애니메이션, 공격을 완료한 후, Idle 상태로 전이
        Dead,       //사망 상태
    }
    public abstract class ObjectAI : MonoBehaviour
    {
        protected NavMeshAgent agent;

        [Header("Target Object")]
        [SerializeField] protected Transform target;
        [SerializeField] protected string targetTag;

        [Header("Object Status")]
        [SerializeField] protected ObjectStatus status;
        protected int currentHP;
        [SerializeField] protected FSM fsm;

        [Header("Debug Mode Enable")]
        [SerializeField] private bool debugModeEnable = true;

        //BATTLE
        protected Transform targetEnemy;
        protected bool isAttack = false;

        [Header("Test Code")]
        [SerializeField] private Vector3 pos;

        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            //currentHP = status.hp;
            currentHP = 10;
        }
        private void Update()
        {
            switch (fsm)
            {
                case FSM.Idle:
                    IdleBehavior();
                    break;
                case FSM.Encounter:
                    EncounterBehavior();
                    break;
                case FSM.Attack:
                    if(isAttack == false)
                        AttackBehavior();
                    break;
                case FSM.Dead:
                    Destroy(gameObject);
                    break;
            }
        }

        protected abstract void IdleBehavior();
        protected abstract void EncounterBehavior();
        protected void AttackBehavior()
        {
            StartCoroutine(AttackStart());
        }
        //테스트 코드입니다.
        protected IEnumerator AttackStart()
        {
            //공격시간 대기
            isAttack = true;
            yield return new WaitForSeconds(status.attackSpeed);
            //박스 생성
            Collider[] targets = CheckAttackCollider(targetEnemy, status.hitboxRange, targetTag);
            if (targets != null)
            {
                foreach (Collider target in targets)
                {
                    if (!target.TryGetComponent<ObjectAI>(out var obj))
                        continue;
                    obj.Hitted(status.damage);
                }
            }
            isAttack = false;
            fsm = FSM.Encounter;
        }
        public void Hitted(int damage)
        {
            currentHP -= damage;
            if(currentHP <= 0)
                fsm = FSM.Dead;
        }
        protected Collider[] CheckAttackCollider(Transform target, float range, string targetTag)
        {
            if (target == null)
                return null;

            Vector3 direction = target.transform.position - transform.position;
            direction = direction.normalized * range;

            Collider[] hitColliders = Physics.OverlapBox(transform.position + direction,
                new Vector3(range, range, range));
            hitColliders.Select(x => x.CompareTag(targetTag)).ToArray();

            return hitColliders;
        }
        protected Transform CheckRange(float range, string targetTag)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, range);
            foreach (Collider collider in hitColliders)
            {
                if (collider.CompareTag(targetTag))
                {
                    return collider.gameObject.transform;
                }
            }
            return null;
        }
        //Debug Code
        private void OnDrawGizmos()
        {
            if (debugModeEnable == false)
                return;

            DrawGizmosCircle(Color.green, status.recognizeRange);
            DrawGizmosCircle(Color.red, status.attackRange);

            var range = status.hitboxRange;
            Gizmos.DrawWireCube(transform.position + pos, new Vector3(range, range, range));
        }
        private void DrawGizmosCircle(Color color, float radius)
        {
            Gizmos.color = color;
            Vector3 previousPoint = Vector3.zero;
            int segments = 36;
            
            for (int i = 0; i <= segments; i++)
            {
                float angle = i * Mathf.PI * 2 / segments;
                Vector3 newPoint = new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);

                if (i > 0)
                    Gizmos.DrawLine(transform.position + previousPoint, transform.position + newPoint);

                previousPoint = newPoint;
            }
        }
    }
}
