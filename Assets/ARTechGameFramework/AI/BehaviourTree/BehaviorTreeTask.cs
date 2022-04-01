namespace ARTech.GameFramework.AI
{
    public abstract class BehaviorTreeTask
    {
        public bool IsRunning { get; protected set; }

        public bool TryInvoke()
        {
            if (IsRunning) return false;

            Invoke();

            return true;
        }

        protected abstract void Invoke();
    }
}