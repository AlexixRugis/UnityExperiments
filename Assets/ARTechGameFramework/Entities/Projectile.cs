using System;
using UnityEngine;

namespace ARTech.GameFramework
{
    public abstract class Projectile : TransformableObject, IProjectile
    {
        [SerializeField] private float _speed = 5f;
        [SerializeField] private float _damage = 10f;
        [SerializeField] private float _lifetime = 10f;
        [SerializeField] private float _hitCheckRadius = 0.5f;
        [SerializeField] private LayerMask _hitMask;

        private float _launchTime;

        public IEntity Shooter { get; set; }
        public float Speed { get => _speed; set => _speed = value; }
        public float Damage { get => _damage; set => _damage = value; }
        public float Lifetime { get => _lifetime; set => _lifetime = value; }
        public float RemainingLifetime => Lifetime + _launchTime - Time.time;
        public bool Launched { get; private set; }
        public Vector3 Direction { get; private set; }
        public float HitCheckRadius => _hitCheckRadius;
        public LayerMask HitMask => _hitMask;

        public Predicate<IHealth> DamageableTypesPredicate { get; set; } = h => true;

        public void Launch(Vector3 direction)
        {
            _launchTime = Time.time;
            Direction = direction;
            Launched = true;
        }

        private void Update()
        {
            if (Launched)
            {
                HandleUpdate();   
            }
        }

        private void HandleUpdate()
        {

            HandleMovement();

            if (Physics.CheckSphere(transform.position, _hitCheckRadius, _hitMask))
            {
                HandleHit();
            }

            if (RemainingLifetime <= 0)
            {
                Remove();
            }
        }

        protected abstract void HandleMovement();
        protected abstract void HandleHit();

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(Position, _hitCheckRadius);
        }
    }
}