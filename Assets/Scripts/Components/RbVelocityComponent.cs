using UnityEngine;

namespace Mobs
{
    [RequireComponent(typeof(Rigidbody))]
    public class RbVelocityComponent : VelocityComponent
    {
        private Rigidbody _rb;

        private void Awake() => _rb = GetComponent<Rigidbody>();

        protected override void ApplyVelocity() => _rb.velocity = Velocity;
    }
}