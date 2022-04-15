using System;
using UnityEngine;

namespace ARTech.GameFramework.AI
{
    public class AvoidTypesNode : Node
    {
        private readonly INPC _host;
        private readonly IMovement _agent;
        private readonly float _sqrCheckDistance;
        private readonly Predicate<ICharacter> _match;
        private readonly float _runSpeed;

        private ICharacter _avoidTarget;

        public AvoidTypesNode(INPC host, IMovement agent, float checkDistance, Predicate<ICharacter> match, float runSpeed)
        {
            _host = host;
            _agent = agent;
            _sqrCheckDistance = checkDistance * checkDistance;
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
                        _avoidTarget.Position,
                        2f
                    )
                );
            }

            ICharacter target = _host.GetNearest(_match);
            if (target == null)
            {
                _avoidTarget = null;
            }
            else if ((_host.Position - target.Position).sqrMagnitude < _sqrCheckDistance)
            {
                _avoidTarget = target;
            }

            return _avoidTarget != null ? NodeState.Running : NodeState.Failure;
        }

        
    }
}