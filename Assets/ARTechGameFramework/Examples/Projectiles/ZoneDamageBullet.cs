using UnityEngine;

namespace ARTech.GameFramework.Examples
{
    public class ZoneDamageBullet : Projectile
    {
        [SerializeField] private float _damageZone;
        [SerializeField] private AnimationCurve _damageFalloff;
        [SerializeField] private LayerMask _damagableMask;

        protected override void HandleHit()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, _damageZone, _damagableMask);

            foreach (var collider in colliders)
            {
                IHealth damagable = collider.GetComponent<IHealth>();
                if (damagable == null || damagable == Shooter) continue;

                if (DamageableTypesPredicate.Invoke(damagable))
                {
                    Vector3 target = collider.ClosestPoint(transform.position);
                    Vector3 direction = (target - transform.position);

                    int damageAmount = (int)(_damageFalloff.Evaluate(direction.magnitude / _damageZone) * Damage);
                    damagable.TakeDamage(damageAmount);
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