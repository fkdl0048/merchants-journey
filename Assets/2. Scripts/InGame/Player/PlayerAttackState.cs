using Abstract;
using PlayerScript;
using System.Collections.Generic;

namespace PlayerState
{
    public class PlayerAttackState : Abstract.PlayerState
    {
        PlayerAnimationController animationController;
        PlayerAttackBehaviour attackBehaviour; 

        int currentindex = 0; //0, 1, 2
        bool isAttack = false;
        public PlayerAttackState(PlayerAnimationController animationController, PlayerAttackBehaviour attackBehaviour) 
        { 
            this.animationController = animationController;
            this.attackBehaviour = attackBehaviour;
        }
        public override void Enter()
        {
            currentindex = 0;
            return;
        }

        public override PlayerSignal Exit()
        {
            return PlayerSignal.None;
        }

        public override PlayerSignal OnAction()
        {
            if(animationController.IsPlayEnd() == true)
            {
                animationController.EnableAnimation("isAttack", false);
                return PlayerSignal.AttackEndSignal;
            }
            
            if (isAttack == false && animationController.GetAnimationPercent() >= 0.9f)
            {
                attackBehaviour.Attack();
                isAttack = true;
            }

            animationController.EnableAnimation("isAttack", true);

            return PlayerSignal.None;
        }

        public override PlayerSignal OnActionUpdate()
        {
            PreCheck();

            return OnAction();

            //return PlayerSignal.None;
        }
    }
}