using UnityEngine;

namespace ARTech.GameFramework.Examples
{
    public class DefaultBullet : Projectile
    {
        protected override void HandleHit(Collision collision)
        {
            if (collision.transform == transform) return;

            IDamageable damagable = collision.transform.GetComponent<IDamageable>();
            if (damagable != null && damagable != Shooter && DamageableTypesPredicate.Invoke(damagable))
            {
                damagable.TakeDamage(Damage);
            }

            Destroy(gameObject);
        }

        protected override void HandleMovement()
        {
            transform.LookAt(transform.position + Direction);
            transform.Translate(0, 0, Speed * Time.deltaTime);
        }
    }
}