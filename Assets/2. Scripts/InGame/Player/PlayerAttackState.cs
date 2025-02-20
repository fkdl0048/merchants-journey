using Abstract;
using PlayerScript;
using System.Collections.Generic;

namespace PlayerState
{
    public class PlayerAttackState : Abstract.PlayerState
    {
        PlayerAnimationController animationController;

        int currentindex = 0; //0, 1, 2
        public PlayerAttackState(PlayerAnimationController animationController) 
        { 
            this.animationController = animationController;
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
            return PlayerSignal.None;
        }
    }
}