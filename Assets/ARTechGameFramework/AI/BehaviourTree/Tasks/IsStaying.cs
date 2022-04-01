using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARTech.GameFramework.AI
{
    public class IsStaying : Node
    {
        private AIAgent _agent;

        public IsStaying(AIAgent agent)
        {
            _agent = agent;
        }

        public override NodeState Evaluate()
        {
            return _agent.Velocity < 0.1f ? NodeState.Success : NodeState.Failure;
        }
    }
}