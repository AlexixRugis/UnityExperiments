using System;
using UnityEngine;

namespace ARTech.GameFramework
{
    public abstract class Projectile : MonoBehaviour
    {
        [SerializeField] private float _speed = 5f;
        [SerializeField] private float _damage = 10f;
        [SerializeField] private float _hitCheckRadius = 0.5f;
        [SerializeField] private LayerMask _hitMask;

        public float Speed { get => _speed; set => _speed = value; }
        public float Damage { get => _damage; set => _damage = value; }
        public bool Launched { get; private set; }
        public Vector3 Direction { get; private set; }
        public float HitCheckRadius => _hitCheckRadius;
        public LayerMask HitMask => _hitMask;

        public Predicate<IDamageable> DamageableTypesPredicate { get; set; } = h => true;
        public Character Shooter { get; set; }

        public void Launch(Vector3 direction)
        {
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
        }

        protected abstract void HandleMovement();
        protected abstract void HandleHit();

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _hitCheckRadius);
        }
    }
}