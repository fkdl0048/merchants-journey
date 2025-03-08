namespace Abstract
{
    public enum EnemySignal
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
    public abstract class EnemyState
    {
        protected bool isEnterFunc = false;

        public abstract void Enter();
        public abstract EnemySignal Exit();
        public abstract EnemySignal OnAction();
        public abstract EnemySignal OnActionUpdate();
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