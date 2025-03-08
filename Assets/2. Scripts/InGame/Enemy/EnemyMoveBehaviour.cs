using UnityEngine;

namespace EnemyScript
{
    public class EnemyMoveBehaviour : MonoBehaviour
    {
        [SerializeField] private float speed = 1f;

        public void MoveBehaviour(Transform target)
        {
            if(target != null)
            {
                Vector2 direction = (target.position - transform.position).normalized;
                float angle = Mathf.Atan2(direction.x, -1 * direction.y) * Mathf.Rad2Deg;

                transform.position = Vector2.MoveTowards(
                    transform.position,
                    target.transform.position, speed * Time.deltaTime);

                transform.rotation = Quaternion.Euler(0, 0, angle);
            }
        }

        public bool IsInRange(Transform target, float attackRange)
        {
            if (target == null)
                return false;

            return Vector2.Distance(transform.position, target.position) <= attackRange;
        }
    }
}