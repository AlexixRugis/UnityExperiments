using System;
using UnityEngine;

namespace ARTech.GameFramework.AI
{
    public class AvoidTypesNode : Node
    {
        private readonly Character _host;
        private readonly IMovement _agent;
        private readonly float _checkDistance;
        private readonly Predicate<Character> _match;
        private readonly float _runSpeed;

        private Character _avoidTarget;

        public AvoidTypesNode(Character host, float checkDistance, Predicate<Character> match, float runSpeed)
        {
            _host = host;
            _agent = host.MovementController;
            _checkDistance = checkDistance;
            _match = match;
            _runSpeed = runSpeed;
        }

        public override NodeState Evaluate()
        {
            if (_avoidTarget != null)
            {
                _agent.Speed = _runSpeed;
                _agent.TryMove(
                    _host.GetPositionFrom(
                        _avoidTarget.transform.position,
                        2f
                    )
                );
            }

            _avoidTarget = _host.GetNearest(_match, _checkDistance);

            return _avoidTarget != null ? NodeState.Running : NodeState.Failure;
        }

        
    }
}