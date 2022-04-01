using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARTech.GameFramework.AI
{
    public class Sequence : Node
    {
        private bool _isParallel;

        public Sequence(List<Node> children, bool isParallel = false) : base(children) { _isParallel = isParallel; }
        public override NodeState Evaluate()
        {
            bool isAnyChildRunning = false;

            foreach (var child in Children)
            {
                switch (child.Evaluate())
                {
                    case NodeState.Failure:
                        State = NodeState.Failure;
                        return State;
                    case NodeState.Success:
                        continue;
                    case NodeState.Running:
                        isAnyChildRunning = true;

                        if (!_isParallel)
                        {
                            State = NodeState.Running;
                            return State;
                        }

                        continue;
                    default:
                        State = NodeState.Success;
                        return State;
                }
            }

            State = isAnyChildRunning ? NodeState.Running : NodeState.Success;
            return State;
        }
    }
}