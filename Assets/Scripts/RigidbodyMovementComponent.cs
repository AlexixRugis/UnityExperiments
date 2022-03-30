using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mobs
{
    [RequireComponent(typeof(Rigidbody))]
    public class RigidbodyMovementComponent : MonoBehaviour, IMovement
    {
        private Vector3 _destination;
        public float Speed { get; set; }
        public bool IsRotatingToMovement { get; set; }
        public bool ReachedDestination => _destination == _rigidbody.position;

        private Rigidbody _rigidbody;

        public void SetDestination(Vector3 position)
        {
            _destination = position;
        }

        public void Stop()
        {
            _destination = _rigidbody.position;
        }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            if (!ReachedDestination)
            {
                _rigidbody.MovePosition(Vector3.MoveTowards(_rigidbody.position, _destination, Speed * Time.deltaTime));
            }
        }
    }
}