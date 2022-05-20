using System;
using UnityEngine;

namespace ARTech.GameFramework.AI
{
    public class TeleportFromTypesNode : AIState
    {
        private readonly Character _host;
        private readonly IMovement _agent;
        private readonly float _cooldown;
        private readonly float _checkRadius;
        private readonly Predicate<Character> _match;
        private readonly float _distance;

        private float _lastTeleportTime;
        private Character _target;

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

        public override bool CanEnter()
        {
            if (Time.time - _lastTeleportTime <= _cooldown)
            {
                return false;
            }

            _target = _host.GetNearest(_match, _checkRadius);

            if (_target == null)
            {
                return false;
            }

            return true;
        }

        public override bool CanExit() => true;

        public override AIStateResult Evaluate()
        {
            if (_agent.Teleport(
                _host.GetRandomPositionAround(_distance)
            ))
            {
                _lastTeleportTime = Time.time;
            }

            return AIStateResult.Success;
        }
    }
}