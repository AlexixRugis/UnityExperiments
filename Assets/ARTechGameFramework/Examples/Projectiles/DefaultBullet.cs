using UnityEngine;

namespace ARTech.GameFramework.Examples
{
    public class DefaultBullet : Projectile
    {
        protected override void HandleHit()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, HitCheckRadius, HitMask);
            foreach (var collider in colliders)
            {
                IDamageable damagable = collider.GetComponent<IDamageable>();
                if (damagable != null && damagable != Shooter && DamageableTypesPredicate.Invoke(damagable))
                {
                    damagable.TakeDamage(Damage);
                }
            }

            Remove();
        }

        protected override void HandleMovement()
        {
            transform.LookAt(Position + Direction);
            transform.Translate(0, 0, Speed * Time.deltaTime);
        }
    }
}