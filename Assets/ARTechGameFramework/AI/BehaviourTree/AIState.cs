namespace ARTech.GameFramework.AI
{
    public enum AIStateResult
    {
        Running,
        Success,
    }

    public abstract class AIState
    {
        public abstract bool CanExit();
        public abstract bool CanEnter();
        public virtual void OnEnterState() { }
        public virtual void OnExitState() { }
        public abstract AIStateResult Evaluate();
    }
}