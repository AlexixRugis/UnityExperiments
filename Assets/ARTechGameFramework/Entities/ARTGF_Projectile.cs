using System;
using UnityEngine;

namespace ARTech.GameFramework
{
    public abstract class ARTGF_Projectile : MonoBehaviour
    {
        [SerializeField] private float _speed = 5f;
        [SerializeField] private float _damage = 10f;
        [SerializeField] private float _lifetime = 20f;

        public float Speed { get => _speed; set => _speed = value; }
        public float Damage { get => _damage; set => _damage = value; }
        public bool Launched { get; private set; }
        public Vector3 Direction { get; private set; }

        public Predicate<ARTGF_IDamageable> DamageableTypesPredicate { get; set; } = h => true;
        public ARTGF_Character Shooter { get; set; }

        public void Launch(Vector3 direction)
        {
            Direction = direction;
            Launched = true;
            HandleLaunch();

            if (_lifetime > 0f) Destroy(gameObject, _lifetime);
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
        }

        protected virtual void HandleLaunch() { }
        protected virtual void HandleMovement() { }
        protected abstract void HandleHit(Collision collision);

        private void OnCollisionEnter(Collision collision)
        {
            if (Launched)
                HandleHit(collision);
        }
    }
}