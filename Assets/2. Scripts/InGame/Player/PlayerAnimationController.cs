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

        public bool IsPlayEnd()
        {
            // 현재 애니메이션이 체크하고자 하는 애니메이션인지 확인
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                return true;
            else
                return false;
        }

        public float GetAnimationPercent() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }
}
