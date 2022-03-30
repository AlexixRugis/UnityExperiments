using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mobs
{
    [RequireComponent(typeof(MoveToPositionComponent), typeof(AttackableComponent))]
    public class RunAwayStateComponent : StateComponent
    {
        public float RunDistance;
        public float RunSpeed;

        private MoveToPositionComponent _moveComponent;
        private AttackableComponent _attackableComponent;

        private Transform _locatedAttacker;

        public override void OnEnterState()
        {
            base.OnEnterState();
            if (_moveComponent) _moveComponent.TargetPosition = transform.position;
        }

        private void Awake()
        {
            _moveComponent = GetComponent<MoveToPositionComponent>();
            _attackableComponent = GetComponent<AttackableComponent>();
        }

        public override void OnUpdateState()
        {
            base.OnUpdateState();
            if (_moveComponent.ReachedDestination)
            {
                AttackerComponent attackerComponent = _attackableComponent.GetNearestAttacker(); 
                if (attackerComponent) _locatedAttacker = attackerComponent.transform;

                if (_locatedAttacker)
                {
                    _moveComponent.Speed = RunSpeed;
                    _moveComponent.TargetPosition = GetNextPoint();
                }
            }
        }
        
        private Vector3 GetNextPoint()
        {
            Vector3 direction = (transform.position - _locatedAttacker.position);
            direction.y = 0;
            direction = Quaternion.Euler(0, Random.Range(-75, 75), 0) * direction.normalized;
            float distance = RunDistance;

            if (Physics.Raycast(transform.position, direction, out RaycastHit hit, distance))
            {
                if (hit.transform != transform)
                {
                    distance = hit.distance;
                }
            }

            return transform.position + (direction * distance);
        }
    }
}