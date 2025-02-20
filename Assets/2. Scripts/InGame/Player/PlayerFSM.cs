using Abstract;
using CPlayerState;
using PlayerState;
using Unity.Collections;
using UnityEngine;

namespace PlayerScript
{
    public class PlayerFSM : MonoBehaviour
    {
        [ReadOnly] public Abstract.PlayerState state;

        private PlayerMovement playerMovement;
        private PlayerAnimationController animationController;

        [SerializeField] float speed = 100;
        [SerializeField] float Runspeed = 2;
        [SerializeField] float Dashspeed = 10;

        // Start is called before the first frame update
        void Start()
        {
            playerMovement = GetComponent<PlayerMovement>();
            animationController = GetComponent<PlayerAnimationController>();

            state = new PlayerWalkState(playerMovement, animationController, speed, Runspeed, Dashspeed);
        }

        // Update is called once per frame
        void Update()
        {
            if(state == null)
            {
                return; 
            }

            PlayerSignal signal = state.OnActionUpdate();

            switch(signal)
            {
                case PlayerSignal.None:
                case PlayerSignal.IdleSignal:
                case PlayerSignal.RunSignal:
                    break;
                case PlayerSignal.AttackSignal:
                    state = new PlayerAttackState(animationController);
                    break;
                case PlayerSignal.AttackEndSignal:
                    state = new PlayerWalkState(playerMovement, animationController, speed, Runspeed, Dashspeed);
                    break;
                case PlayerSignal.Error:
                    break;
                
            }
        }
    }

}
