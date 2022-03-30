using UnityEngine;
using System;
using System.Collections;

namespace Mobs
{
    [RequireComponent(typeof(MoveToPositionComponent))]
    public sealed class PatrolStateComponent : StateComponent
    {
        public float MinMovementDistance;
        public float MaxMovementDistance;
        public float PatrolSpeed;

        public float MinStayTime;
        public float MaxStayTime;

        public LayerMask ObstacleMask;

        private MoveToPositionComponent _moveComponent;
        private float _stayTimer;

        private void Awake()
        {
            _moveComponent = GetComponent<MoveToPositionComponent>();
            _moveComponent.TargetPosition = transform.position;
        }

        public override void OnEnterState()
        {
            base.OnEnterState();
            _moveComponent.TargetPosition = transform.position;
        }

        public Vector3 GetNextPoint()
        {
            Vector3 direction = GetMovementDirection();
            float distance = GetMovementDistance();

            if (Physics.Raycast(transform.position, direction, out RaycastHit hit, distance, ObstacleMask))
            {
                if (hit.transform != transform)
                {
                    distance = hit.distance;
                }
            }

            return transform.position + (direction * distance);
        }

        private float GetStayTime() => UnityEngine.Random.Range(MinStayTime, MaxStayTime);
        private float GetMovementDistance() => UnityEngine.Random.Range(MinMovementDistance, MaxMovementDistance);
        private Vector3 GetMovementDirection() => Quaternion.Euler(0, UnityEngine.Random.Range(0, 360), 0) * transform.forward;

        public override void OnUpdateState()
        {
            if (_moveComponent.ReachedDestination)
            {
                if (_stayTimer <= 0)
                {
                    _moveComponent.Speed = PatrolSpeed;
                    _moveComponent.TargetPosition = transform.position + GetMovementDirection() * GetMovementDistance();
                    _stayTimer = GetStayTime();
                }
                else
                {
                    _stayTimer -= Time.deltaTime;
                }
            }
        }
    }
}
