using System;
using UnityEngine;

namespace ARTech.GameFramework.AI
{
    public class TeleportFromTypesNode : Node
    {
        private readonly INPC _host;
        private readonly IMovement _agent;
        private readonly float _cooldown;
        private readonly float _sqrCheckRadius;
        private readonly Predicate<ICharacter> _match;
        private readonly float _distance;

        private float _lastTeleportTime;

        public TeleportFromTypesNode(INPC host, IMovement agent, float checkRadius, Predicate<ICharacter> match, float cooldown, float distance)
        {
            _host = host;
            _agent = agent;
            _cooldown = cooldown;
            _sqrCheckRadius = checkRadius * checkRadius;
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

            ICharacter target = _host.GetNearest(_match);

            if (target == null || (_host.Position - target.Position).sqrMagnitude < _sqrCheckRadius)
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