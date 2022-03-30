using System.Collections.Generic;

namespace ARTech.GameFramework.AI
{
    public class InvertNode : Node
    {
        private Node _child;

        public InvertNode(Node child) : base(new List<Node>() { child }) { _child = child; }

        public override NodeState Evaluate()
        {
            switch(_child.Evaluate())
            {
                case NodeState.Success:
                    State = NodeState.Failure;
                    return State;
                case NodeState.Failure:
                    State = NodeState.Success;
                    return State;
                case NodeState.Running:
                    State = NodeState.Running;
                    return State;
            }

            return NodeState.Success;
        }
    }
}
