using UnityEngine;
using UnityEngine.AI;

namespace ObjectAI
{
    public enum FSM
    {
        Idle,       //화물을 호위하는, 또는 화물에 접근하고 있는 상태
        Encounter,  //적을 맞닥뜨린 상태
        Fight,      //사정거리 안으로 들어온 상태, 공격을 준비한다.
        Attack,     //공격 애니메이션, 공격을 완료한 후, Idle 상태로 전이
        Dead,       //사망 상태
    }
    public abstract class ObjectAI : MonoBehaviour
    {
        [Header("Target Object")]
        [SerializeField] protected Transform target;
        protected NavMeshAgent agent;

        [SerializeField] protected ObjectStatus status;
        [SerializeField] protected FSM fsm;

        [Header("Debug Mode Enable")]
        [SerializeField] private bool debugModeEnable = true;

        
        protected Transform targetEnemy;

        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
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
                case FSM.Fight:
                    break;
                case FSM.Attack:
                    break;
                case FSM.Dead:
                    Destroy(gameObject);
                    break;
            }
        }
        
        protected abstract void IdleBehavior();
        protected abstract void EncounterBehavior();
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
                {
                    Gizmos.DrawLine(transform.position + previousPoint, transform.position + newPoint);
                }

                previousPoint = newPoint;
            }
        }
    }
}
