namespace ARTech.GameFramework.AI
{
    public class WaitNode : Node
    {
        private float _duration;
        private float _lastTime = 0;
        private bool _failing;

        public WaitNode(float duration, bool failing = true)
        {
            _duration = duration;
            _failing = failing;
        }

        public override NodeState Evaluate()
        {
            State = _failing ? NodeState.Failure : NodeState.Running;

            if (UnityEngine.Time.time - _lastTime >= _duration)
            {
                _lastTime = UnityEngine.Time.time;

                State = NodeState.Success;
            }

            return State;
        }
    }
}