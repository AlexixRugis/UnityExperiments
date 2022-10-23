using UnityEngine;

namespace ARTech.GameFramework.Examples
{
    [RequireComponent(typeof(Rigidbody))]
    public class ARTGF_DefaultBullet : ARTGF_Projectile
    {
        private Rigidbody _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.isKinematic = true;
        }

        protected override void HandleHit(Collision collision)
        {
            if (collision.transform == transform) return;

            ARTGF_IDamageable damagable = collision.transform.GetComponent<ARTGF_IDamageable>();
            if (damagable != null && damagable != Shooter && DamageableTypesPredicate.Invoke(damagable))
            {
                damagable.TakeDamage(Damage);
            }

            Destroy(gameObject);
        }

        protected override void HandleMovement()
        {
            _rigidbody.isKinematic = false;
            _rigidbody.velocity = Direction.normalized * Speed;
        }
    }
}