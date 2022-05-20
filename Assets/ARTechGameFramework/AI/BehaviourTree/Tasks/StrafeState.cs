using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARTech.GameFramework.AI
{
    public class StrafeState : AIState
    {
        private readonly Character _host;
        private readonly IMovement _agent;
        private readonly float _radius;
        private readonly float _speed;
        private readonly float _time;

        private float _lastStrafeTime;

        public StrafeState(Character host, float radius, float speed, float time)
        {
            _host = host;
            _agent = host.MovementController;
            _radius = radius;
            _speed = speed;
            _time = time;
            _lastStrafeTime = 0;
        }

        public override bool CanEnter() => _host.BattleTarget;

        public override bool CanExit() => true;

        public override AIStateResult Evaluate()
        {
            if (!_host.BattleTarget) return AIStateResult.Success;

            if (Time.time - _lastStrafeTime > _time)
            {
                if (!_agent.HasPath())
                {
                    _agent.TryMove(
                        _host.GetRandomPositionAround(_radius)
                    );
                }
                else
                {
                    _lastStrafeTime = Time.time;

                    Vector3 direction = _host.BattleTarget.transform.position - _host.transform.position;
                    direction.y = 0;
                    _host.transform.rotation = Quaternion.LookRotation(direction);
                }
            }

            return AIStateResult.Running;
        }
    }
}