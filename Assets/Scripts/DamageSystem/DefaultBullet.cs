using ARTech.GameFramework;
using UnityEngine;

namespace Mobs
{
    public class DefaultBullet : Bullet
    {
        protected override void HandleHit()
        {
            base.HandleHit();

            Collider[] colliders = Physics.OverlapSphere(transform.position, HitCheckDistance, HitMask);
            foreach (var collider in colliders)
            {
                IDamageable damagable = collider.GetComponent<IDamageable>();
                if (damagable != null)
                {
                    damagable.TakeDamage(Damage);
                }
            }
        }
    }
}