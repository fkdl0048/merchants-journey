using UnityEngine;


namespace PlayerScript
{
    public class PlayerAnimationController : MonoBehaviour
    {
        private Animator animator;

        private void Start()
        {
            animator = GetComponent<Animator>();
        }
        public void EnableAnimation(string parameterName, bool enable)
        {
            if (animator.GetBool(parameterName) == enable)
                return;
            animator.SetBool(parameterName, enable);
        }
    }
}
