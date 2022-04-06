using System;
using UnityEngine;

namespace ARTech.GameFramework.AI
{
    public class TeleportFromTypesNode : Node
    {
        private readonly ILivingEntity _entity;
        private readonly IMovement _agent;
        private readonly float _cooldown;
        private readonly float _checkRadius;
        private readonly Type[] _entityTypes;
        private readonly float _distance;

        private float _lastTeleportTime;

        public TeleportFromTypesNode(ILivingEntity entity, IMovement agent, float checkRadius, Type[] entityTypes, float cooldown, float distance)
        {
            _entity = entity;
            _agent = agent;
            _cooldown = cooldown;
            _checkRadius = checkRadius;
            _entityTypes = entityTypes;
            _distance = distance;

            _lastTeleportTime = 0;
        }

        public override NodeState Evaluate()
        {
            if (Time.time - _lastTeleportTime <= _cooldown)
            {
                return NodeState.Failure;
            }

            IEntity target = _entity.GetNearest(_checkRadius, _entityTypes);

            if (target == null)
            {
                return NodeState.Failure;
            }

            if (_agent.Teleport(
                _agent.GetRandomPositionAround(target.Position, _distance)
            ))
            {
                _lastTeleportTime = Time.time;
                return NodeState.Success;
            }

            return NodeState.Failure;
        }
    }
}