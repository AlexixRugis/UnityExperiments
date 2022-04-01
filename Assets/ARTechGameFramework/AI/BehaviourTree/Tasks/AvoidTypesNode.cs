using System;
using UnityEngine;

namespace ARTech.GameFramework.AI
{
    public class AvoidTypesNode : Node
    {
        private readonly ILivingEntity _entity;
        private readonly IMovement _agent;
        private readonly float _checkDistance;
        private readonly Type[] _entityTypes;
        private readonly float _runSpeed;

        private IEntity _avoidTarget;

        public AvoidTypesNode(ILivingEntity entity, IMovement agent, float checkDistance, Type[] entityTypes, float runSpeed)
        {
            _entity = entity;
            _agent = agent;
            _checkDistance = checkDistance;
            _entityTypes = entityTypes;
            _runSpeed = runSpeed;
        }

        public override NodeState Evaluate()
        {
            if (_avoidTarget != null)
            {
                _agent.Speed = _runSpeed;
                _agent.TryMove(
                    _agent.GetPositionFrom(
                        _entity.Location,
                        _avoidTarget.Location,
                        2f
                    )
                );
            }

            _avoidTarget = _entity.GetNearest(_checkDistance, _entityTypes);
            return _avoidTarget != null ? NodeState.Running : NodeState.Failure;
        }

        
    }
}