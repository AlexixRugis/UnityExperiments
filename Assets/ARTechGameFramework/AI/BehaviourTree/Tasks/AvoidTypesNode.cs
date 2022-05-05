using System;
using UnityEngine;

namespace ARTech.GameFramework.AI
{
    public class AvoidTypesNode : Node
    {
        private readonly AICharacter _host;
        private readonly IMovement _agent;
        private readonly float _checkDistance;
        private readonly Predicate<AICharacter> _match;
        private readonly float _runSpeed;

        private AICharacter _avoidTarget;

        public AvoidTypesNode(AICharacter host, float checkDistance, Predicate<AICharacter> match, float runSpeed)
        {
            _host = host;
            _agent = host.MovementController;
            _checkDistance = checkDistance * checkDistance;
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

            AICharacter target = _host.GetNearest(_match, _checkDistance);
            if (target == null)
            {
                _avoidTarget = null;
            }
            else if ((_host.transform.position - target.transform.position).sqrMagnitude < _checkDistance)
            {
                _avoidTarget = target;
            }

            return _avoidTarget != null ? NodeState.Running : NodeState.Failure;
        }

        
    }
}