using System;
using UnityEngine;

namespace ARTech.GameFramework.AI
{
    public class AvoidTypesNode : Node
    {
        private readonly ILivingEntity _entity;
        private readonly IMovement _agent;
        private readonly float _checkDistance;
        private readonly Predicate<IEntity> _match;
        private readonly float _runSpeed;

        private IEntity _avoidTarget;

        public AvoidTypesNode(ILivingEntity entity, IMovement agent, float checkDistance, Predicate<IEntity> match, float runSpeed)
        {
            _entity = entity;
            _agent = agent;
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
                    _agent.GetPositionFrom(
                        _entity.Position,
                        _avoidTarget.Position,
                        2f
                    )
                );
            }

            _avoidTarget = _entity.GetNearest(_checkDistance, _match);
            return _avoidTarget != null ? NodeState.Running : NodeState.Failure;
        }

        
    }
}