using ARTech.GameFramework;
using UnityEngine;

namespace Mobs
{
    [RequireComponent(typeof(Rigidbody))]
    public sealed class Bomb : ARTGF_Projectile
    {
        [SerializeField] private float _damageZone;
        [SerializeField] private AnimationCurve _damageFalloff;
        [SerializeField] private LayerMask _damagableMask;

        private Rigidbody _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.isKinematic = true;
        }

        protected override void HandleHit(Collision collision)
        {
            if (collision.transform == transform) return;

            Collider[] colliders = Physics.OverlapSphere(transform.position, _damageZone, _damagableMask);

            foreach (var collider in colliders)
            {
                ARTGF_IDamageable damagable = collider.GetComponent<ARTGF_IDamageable>();
                if (damagable == null || damagable == Shooter) continue;

                if (DamageableTypesPredicate.Invoke(damagable))
                {
                    Vector3 target = collider.ClosestPoint(transform.position);
                    Vector3 direction = (target - transform.position);

                    int damageAmount = (int)(_damageFalloff.Evaluate(direction.magnitude / _damageZone) * Damage);
                    damagable.TakeDamage(damageAmount);
                }
            }

            Destroy(gameObject);
        }

        protected override void HandleLaunch()
        {
            base.HandleLaunch();
            _rigidbody.isKinematic = false;
            _rigidbody.velocity = Direction.normalized * Speed;
        }
    }
}