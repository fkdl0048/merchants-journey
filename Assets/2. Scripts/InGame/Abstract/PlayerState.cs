namespace Abstract
{
    public enum PlayerSignal
    {
        None,
        IdleSignal,
        WalkSignal,
        RunSignal,
        AttackSignal,
        //AttackTwoSignal,
        //AttackSecondSignal,
        AttackEndSignal,
        Error
    }
    public abstract class PlayerState
    {
        protected bool isEnterFunc = false;

        public abstract void Enter();
        public abstract PlayerSignal Exit();
        public abstract PlayerSignal OnAction();
        public abstract PlayerSignal OnActionUpdate();
        public void PreCheck()
        {
            if(!isEnterFunc)
            {
                isEnterFunc = true;
                Enter();
            }
        }
    }
}