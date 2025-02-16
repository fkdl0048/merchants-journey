using UnityEngine;
using UnityEngine.UIElements;

namespace AI
{
    public class PlayerAI : ObjectAI
    {
        [SerializeField] private bool isForce = false; //이동 명령이 우선시 됨.
        private LineRenderer lineRenderer;
        public Tile myTile;
        public void ChangeForce(bool isForce) => this.isForce = isForce;
        public void Setup(Cargo cargo ,Vector3 targetPosition, bool aiEnable, Tile tile)
        {
            Setup(cargo, targetPosition, aiEnable);
            myTile = tile;
            lineRenderer = GetComponent<LineRenderer>();

            lineRenderer.startWidth = 0.5f;
            lineRenderer.endWidth = 0.5f;
            lineRenderer.positionCount = 0;
        }
        protected override void EncounterBehavior()
        {
            if (targetEnemy == null || isForce)
            {
                fsm = FSM.Idle;
                return;
            }

            HidePath();
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
            if (agent.hasPath && Vector3.Distance(agent.destination, transform.position) > agent.stoppingDistance)
                DrawPath();
            else
                HidePath();
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
        protected override void AttackBehavior()
        {
            StartCoroutine(AttackStart());
        }
        protected override void DestoryBehavior()
        {
            myTile.hasUnit = false;
            Destroy(gameObject);
        }
        private bool IsArrive()
        {
            var target = cargo.transform.position + targetPosition;
            Vector2 a = CMath.RoundVector2Data(new(gameObject.transform.position.x, gameObject.transform.position.z));
            Vector2 t = CMath.RoundVector2Data(new(target.x, target.z)); 
            return a == t;
        }
        private void DrawPath()
        {
            lineRenderer.positionCount = agent.path.corners.Length;
            lineRenderer.SetPosition(0, transform.position);

            if (agent.path.corners.Length < 2)
                return;

            for(int i = 1; i < agent.path.corners.Length; i++)
            {
                var pointPosition = new Vector3(
                    agent.path.corners[i].x,
                    agent.path.corners[i].y,
                    agent.path.corners[i].z);

                lineRenderer.SetPosition(i, pointPosition);
            }
        }
        private void HidePath()
        {
            lineRenderer.positionCount = 0;
        }
    }
}
