using Scripts.InGame.Stage;
using Scripts.Manager;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace AI
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
        protected Cargo cargo; 
        public bool aiEnable;

        [Header("Target Object")]
        [SerializeField] protected Vector3 targetPosition; //navMesh가 추적하는 기존 포지션
        [SerializeField] protected string targetTag;

        [Header("Object Status")]
        [SerializeField] protected ObjectStatus status;
        [SerializeField] protected int currentHP;
        [SerializeField] protected FSM fsm;
        protected SpriteRenderer spriteRenderer;

        [Header("Sound Clip")]
        [SerializeField] private AudioClip hitSoundClip;
        [SerializeField] private AudioClip hittedSoundClip;

        [Header("Debug Mode Enable")]
        [SerializeField] private bool debugModeEnable = true;

        //BATTLE
        [Header("Debug")]
        [SerializeField] protected Transform targetEnemy;   //navMesh가 추적하는 적군 포지션
        protected bool isAttack = false;

        [Header("Test Code")]
        [SerializeField] private Vector3 pos;

        public void Setup(Cargo cargo, Vector3 targetPosition, bool aiEnable)
        {
            this.cargo = cargo;
            this.aiEnable = aiEnable;

            ChangeTargetPostion(targetPosition);
        }
        public void ChangeTargetPostion(Vector3 targetPosition)
        {
            this.targetPosition = targetPosition - cargo.transform.position;
        }
        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            spriteRenderer = GetComponent<SpriteRenderer>();

            currentHP = status.hp;
        }
        private void Update()
        {
            if (aiEnable == false)
                return;

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
            //공격 사운드 재생
            AudioManager.Instance.PlaySFX(hitSoundClip);
            //박스 생성
            Collider[] targets = CheckAttackCollider(status.hitboxRange, targetTag);
            if (targets != null)
            {
                foreach (Collider target in targets)
                {
                    if (!target.TryGetComponent<ObjectAI>(out var obj))
                        continue;
                    obj.Hitted(status.damage, transform.position);
                }
            }
            isAttack = false;
            fsm = FSM.Encounter;
        }
        public void Hitted(int damage, Vector3 hitterPos)
        {
            AudioManager.Instance.PlaySFX(hittedSoundClip);

            currentHP -= damage;
            if(currentHP <= 0)
                fsm = FSM.Dead;

            //StartCoroutine(KnockBack(hitterPos, damage));
        }
        private IEnumerator KnockBack(Vector3 hitterPos, float force)
        {
            TryGetComponent<Rigidbody>(out var rig);
            if (rig == null)
                yield return null;
            else
            {
                Vector3 dir = -1 * ((transform.position - hitterPos).normalized);
                rig.isKinematic = false;
                rig.AddForce(dir * force, ForceMode.Impulse);
                //번쩍이는 효과 적용
                StartCoroutine(EnableFlashEffect());
                yield return new WaitForSeconds(0.25f);
                //복원
                rig.isKinematic = true;
            }
        }
        private IEnumerator EnableFlashEffect()
        {
            var material = spriteRenderer.material;
            if (material == null)
                yield return null;

            material.SetFloat("_FlashAmount", 1.0f);

            yield return new WaitForSeconds(0.5f);

            material.SetFloat("_FlashAmount", 0.0f);
        }
        protected Collider[] CheckAttackCollider(float range, string targetTag)
        {
            if (targetEnemy == null)
                return null;

            Vector3 direction = targetEnemy.transform.position - transform.position;
            direction = direction.normalized * range;

            Collider[] hitColliders = Physics.OverlapBox(transform.position + direction,
                new Vector3(range, range, range));
            Collider[] results = hitColliders.Where(x => x.CompareTag(targetTag)).Select(x => x).ToArray();

            return results;
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

        }
        private void OnDrawGizmosSelected()
        {
            DrawGizmosHitboxRange(Color.gray, status.hitboxRange);
        }
        private void DrawGizmosCircle(Color color, float radius)
        {
            Gizmos.color = color;
            Vector3 previousPoint = Vector3.zero;
            int segments = 36;
            
            for (int i = 0; i <= segments; i++)
            {
                float angle = i * Mathf.PI * 2 / segments;
                Vector3 newPoint = new (Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);

                if (i > 0)
                    Gizmos.DrawLine(transform.position + previousPoint, transform.position + newPoint);

                previousPoint = newPoint;
            }
        }
        private void DrawGizmosHitboxRange(Color color, float radius)
        {
           if(targetEnemy == null) return;

            Vector3 direction = targetEnemy.transform.position - transform.position;
            direction = direction.normalized * radius;

            Gizmos.DrawWireCube(transform.position + direction, new Vector3(radius, radius, radius));
        }
    }
}
