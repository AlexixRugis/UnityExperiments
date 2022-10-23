namespace ARTech.GameFramework.AI
{
    public enum ARTGF_AIStateResult
    {
        Running,
        Success,
    }

    public abstract class ARTGF_AIState
    {
        public abstract bool CanExit();
        public abstract bool CanEnter();
        public virtual void OnEnterState() { }
        public virtual void OnExitState() { }
        public abstract ARTGF_AIStateResult Evaluate();
    }
}