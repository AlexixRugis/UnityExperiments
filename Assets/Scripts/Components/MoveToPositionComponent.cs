using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mobs
{
    [RequireComponent(typeof(Rigidbody))]
    public sealed class MoveToPositionComponent : MonoBehaviour
    {
        public float Accuracy;
        public Vector3 TargetPosition;
        public float Speed;

        public bool ReachedDestination => (transform.position - TargetPosition).sqrMagnitude < Accuracy;

        private Rigidbody _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            _rigidbody.MovePosition(Vector3.MoveTowards(transform.position, TargetPosition, Speed*Time.deltaTime));
        }
    }
}