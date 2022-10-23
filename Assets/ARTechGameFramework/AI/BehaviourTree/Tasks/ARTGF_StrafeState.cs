using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARTech.GameFramework.AI
{
    public class ARTGF_StrafeState : ARTGF_AIState
    {
        private readonly ARTGF_Character _host;
        private readonly ARTGF_IMovement _agent;
        private readonly float _targetDistance;
        private readonly float _speed;
        private readonly float _time;

        private float _lastChangeDirectionTime;
        private Vector2 _direction;

        public ARTGF_StrafeState(ARTGF_Character host, float targetDistance, float speed, float time)
        {
            _host = host;
            _agent = host.MovementController;
            _targetDistance = targetDistance;
            _speed = speed;
            _time = time;
            _lastChangeDirectionTime = 0;
        }

        public override bool CanEnter() => _host.BattleTarget;

        public override bool CanExit() => true;

        public override void OnExitState()
        {
            base.OnExitState();
            _agent.FocusPoint = null;
        }

        public override ARTGF_AIStateResult Evaluate()
        {
            if (!_host.BattleTarget) return ARTGF_AIStateResult.Success;

            Vector3 direction = _host.BattleTarget.transform.position - _host.transform.position;
            float sqrDistance = direction.sqrMagnitude;
            direction.Normalize();

            _agent.FocusPoint = _host.BattleTarget.transform.position;
            _agent.Speed = _speed;
            if (sqrDistance < _targetDistance * _targetDistance - 2f)
            {
                _agent.TryMove(_host.transform.position - direction);
            }
            else if (sqrDistance > _targetDistance * _targetDistance + 2f)
            {
                _agent.TryMove(_host.transform.position + direction);
            }
            else
            {
                Vector3 strafeDirectionRight = Vector3.Cross(direction, Vector3.up).normalized;
                _agent.TryMove(_host.transform.position +
                    strafeDirectionRight * _direction.x +
                    Vector3.up * _direction.y);
            }

            if (Time.time - _lastChangeDirectionTime > _time)
            {
                _direction = Random.insideUnitCircle.normalized;
                _lastChangeDirectionTime = Time.time;
            }

            return ARTGF_AIStateResult.Running;
        }
    }
}