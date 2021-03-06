using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARTech.GameFramework
{
    [RequireComponent(typeof(Rigidbody), typeof(Character))]
    public class FlyAgent : MonoBehaviour, IMovement
    {
        [SerializeField] private float _radius;
        [SerializeField] private LayerMask _obstaclesMask;
        [SerializeField] private float _stoppingDistance;
        [SerializeField] private float _speed;
        [SerializeField] private float _acceleration;

        private Rigidbody _rigidbody;

        private Vector3? _target = null;

        private Vector3 _currentVelocity;

        public float Speed { get => _speed; set => _speed = value; }

        public Vector3 CurrentVelocity => _rigidbody.velocity;

        public Character Character { get; private set; }

        public float AgentRadius => _radius;

        public Vector3 DestinationPoint => _target == null ? Vector3.positiveInfinity : _target.Value;

        public Vector3? FocusPoint { get; set; }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.isKinematic = true;
            Character = GetComponent<Character>();
        }

        private void FixedUpdate()
        {
            
            if (_target != null)
            {
                _rigidbody.MovePosition(Vector3.MoveTowards(_rigidbody.position, _target.Value, _speed * Time.fixedDeltaTime));

                if (FocusPoint == null)
                {
                    Vector3 lookDirection = _target.Value - _rigidbody.position;
                    lookDirection.y = 0;
                    _rigidbody.rotation = Quaternion.Slerp(_rigidbody.rotation,
                    Quaternion.LookRotation(lookDirection, Vector3.up), Time.deltaTime * 10f);
                }
                else
                {
                    Vector3 lookDirection = FocusPoint.Value - _rigidbody.position;
                    lookDirection.y = 0;
                    _rigidbody.rotation = Quaternion.Slerp(_rigidbody.rotation,
                    Quaternion.LookRotation(lookDirection, Vector3.up), Time.deltaTime * 10f);
                }

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