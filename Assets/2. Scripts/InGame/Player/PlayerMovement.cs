using UnityEngine;

namespace PlayerScript
{
    public class PlayerMovement : MonoBehaviour
    {
        public enum MoveState
        {
            None,
            Walk,
            Run,
        }
        private float speedX, speedY;
        private Rigidbody2D rigd;
        private float angle;

        private void Start()
        {
            rigd = GetComponent<Rigidbody2D>();
            rigd.isKinematic = true;
        }
        public void MoveBehaviour(float speedX, float speedY, float speed)
        {
            speedX = Input.GetAxisRaw("Horizontal") * speed;
            speedY = Input.GetAxisRaw("Vertical") * speed;

            rigd.velocity = new Vector2(speedX * Time.deltaTime, speedY * Time.deltaTime);

            angle = Mathf.Atan2(speedX, -1 * speedY) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}

