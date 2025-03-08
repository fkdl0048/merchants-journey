using Abstract;
using PlayerScript;
using UnityEngine;

namespace CPlayerState
{
    public class PlayerWalkState : Abstract.PlayerState
    {
        PlayerMovement playerMovement;
        PlayerAnimationController playerAnimationController;

        string runStr = "isRun";
        string walkStr = "isWalk";
        string dashStr = "isDash";

        KeyCode runKey = KeyCode.LeftShift;
        KeyCode dashKey = KeyCode.Space;

        float speed;
        float speedRun;
        float speedDash;

        float speedX;
        float speedY;

        public PlayerWalkState(PlayerMovement playerMovement, PlayerAnimationController playerAnimationController, float speed, float speedRun, float speedDash)
        {
            this.playerMovement = playerMovement;
            this.playerAnimationController = playerAnimationController;
            this.speed = speed;
            this.speedDash = speedDash;
            this.speedRun= speedRun;
        }
        public override void Enter()
        {
            return;
        }

        public override PlayerSignal Exit()
        {
            return PlayerSignal.None;
        }

        public override PlayerSignal OnAction()
        {
            return PlayerSignal.None;
        }

        public override PlayerSignal OnActionUpdate()
        {
            PreCheck();

            speedX = Input.GetAxisRaw("Horizontal") * speed;
            speedY = Input.GetAxisRaw("Vertical") * speed;

            //attack
            if (Input.GetMouseButtonDown(0))
            {
                playerAnimationController.EnableAnimation(walkStr, false);
                playerAnimationController.EnableAnimation(runStr, false);

                return PlayerSignal.AttackSignal;
            }

            //idle
            if (speedX == 0 && speedY == 0)
            {
                playerAnimationController.EnableAnimation(walkStr, false);
                playerAnimationController.EnableAnimation(runStr, false);
                playerMovement.MoveBehaviour(0, 0, speed);
            }
            else
            {
                if (Input.GetKeyDown(dashKey)) //대쉬
                {
                    playerMovement.MoveBehaviour(speedX, speedY, speed * speedDash);
                }
                else if (Input.GetKey(runKey)) //달리기
                {
                    playerAnimationController.EnableAnimation(runStr, true);
                    playerMovement.MoveBehaviour(speedX, speedY, speed * speedRun);
                }
                else
                {
                    playerMovement.MoveBehaviour(speedX, speedY, speed);
                    playerAnimationController.EnableAnimation(walkStr, true);
                    playerAnimationController.EnableAnimation(runStr, false);
                }
            }

            return PlayerSignal.None;
        }
    }
}