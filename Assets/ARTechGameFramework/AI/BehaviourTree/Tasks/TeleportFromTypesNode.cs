using System;
using UnityEngine;

namespace ARTech.GameFramework.AI
{
    public class TeleportFromTypesNode : Node
    {
        private readonly Character _host;
        private readonly IMovement _agent;
        private readonly float _cooldown;
        private readonly float _checkRadius;
        private readonly Predicate<Character> _match;
        private readonly float _distance;

        private float _lastTeleportTime;

        public TeleportFromTypesNode(Character host, float checkRadius, Predicate<Character> match, float cooldown, float distance)
        {
            _host = host;
            _agent = host.MovementController;
            _cooldown = cooldown;
            _checkRadius = checkRadius;
            _match = match;
            _distance = distance;

            _lastTeleportTime = 0;
        }

        public override NodeState Evaluate()
        {
            if (Time.time - _lastTeleportTime <= _cooldown)
            {
                return NodeState.Failure;
            }

            Character target = _host.GetNearest(_match, _checkRadius);

            if (target == null)
            {
                return NodeState.Failure;
            }

            if (_agent.Teleport(
                _host.GetRandomPositionAround(_distance)
            ))
            {
                _lastTeleportTime = Time.time;
                return NodeState.Success;
            }

            return NodeState.Failure;
        }
    }
}