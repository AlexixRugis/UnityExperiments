using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARTech.GameFramework
{
    [RequireComponent(typeof(Rigidbody), typeof(ICharacter))]
    public class FlyAgent : MonoBehaviour, IMovement
    {
        [SerializeField] private float _radius;
        [SerializeField] private LayerMask _obstaclesMask;
        [SerializeField] private float _stoppingDistance;
        [SerializeField] private float _speed;

        private Rigidbody _rigidbody;

        private Vector3? _target = null;

        public float Speed { get => _speed; set => _speed = value; }

        public Vector3 CurrentVelocity => _rigidbody.velocity;

        public ICharacter Character { get; private set; }

        public float AgentRadius => _radius;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            Character = GetComponent<ICharacter>();
        }

        private void FixedUpdate()
        {
            if (_target != null)
            {
                _rigidbody.MovePosition(Vector3.MoveTowards(_rigidbody.position, _target.Value, _speed * Time.fixedDeltaTime));

                if ((_rigidbody.position - _target.Value).sqrMagnitude < _stoppingDistance * _stoppingDistance)
                {
                    _target = null;
                }
            }
        }

        public void ClearPath()
        {
            _target = null;
        }

        public float GetRemainingDistance()
        {
            if (_target == null) return float.PositiveInfinity;

            return Vector3.Distance(transform.position, _target.Value);
        }

        public bool HasPath()
        {
            return _target != null;
        }

        public bool Teleport(Vector3? position)
        {
            if (position == null)
            {
                return false;
            }

            if (!Physics.CheckSphere(position.Value, _radius, _obstaclesMask))
            {
                _rigidbody.position = position.Value;
                return true;
            }

            return false;
        }

        public bool TryMove(Vector3? position)
        {
            if (position == null)
            {
                _target = null;
                return false;
            }

            Vector3 direction = position.Value - transform.position;
            if (!Physics.SphereCast(transform.position, _radius, direction.normalized, out RaycastHit hit, direction.magnitude, _obstaclesMask)) {
                _target = position;
                return true;
            }

            return false;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, _radius);
        }
    }
}