using System;
using UnityEngine;

namespace ARTech.GameFramework.AI
{
    public class ARTGF_TeleportFromTypesNode : ARTGF_AIState
    {
        private readonly ARTGF_Character _host;
        private readonly ARTGF_IMovement _agent;
        private readonly float _cooldown;
        private readonly float _checkRadius;
        private readonly Predicate<ARTGF_Character> _match;
        private readonly float _distance;

        private float _lastTeleportTime;
        private ARTGF_Character _target;

        public ARTGF_TeleportFromTypesNode(ARTGF_Character host, float checkRadius, Predicate<ARTGF_Character> match, float cooldown, float distance)
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

        public override ARTGF_AIStateResult Evaluate()
        {
            if (_agent.Teleport(
                _host.GetRandomPositionAround(_distance)
            ))
            {
                _lastTeleportTime = Time.time;
            }

            return ARTGF_AIStateResult.Success;
        }
    }
}