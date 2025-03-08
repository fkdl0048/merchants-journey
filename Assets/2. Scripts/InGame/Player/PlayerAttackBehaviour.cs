using UnityEngine;

namespace PlayerScript
{
    public class PlayerAttackBehaviour : MonoBehaviour
    {
        [SerializeField] private float damage;
        [SerializeField] private Transform hitbox;
        public void Attack()
        {
            Collider2D collider = Physics2D.OverlapBox(hitbox.position, hitbox.localScale, 0, LayerMask.GetMask("Enemy"));
            if (collider != null)
            {
                Debug.Log(collider.gameObject.name);
                Destroy(collider.gameObject);
            }
        }
    }
}