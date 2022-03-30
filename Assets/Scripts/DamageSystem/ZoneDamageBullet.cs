using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mobs
{
    public class ZoneDamageBullet : Bullet
    {
        [SerializeField] private float _damageZone;
        [SerializeField] private AnimationCurve _damageFalloff;
        [SerializeField] private LayerMask _damagableMask;

        private float _curveLength;

        protected virtual void Awake()
        {
            _curveLength = _damageFalloff[_damageFalloff.length - 1].time;
        }

        protected override void HandleHit()
        {
            base.HandleHit();

            Collider[] colliders = Physics.OverlapSphere(transform.position, _damageZone, _damagableMask);

            foreach (var collider in colliders)
            {
                IDamagable damagable = collider.GetComponent<IDamagable>();
                if (damagable == null) continue;

                Vector3 target = collider.ClosestPoint(transform.position);
                Vector3 direction = (target - transform.position);

                int damageAmount = (int)(_damageFalloff.Evaluate(direction.magnitude / _damageZone) * Damage);
                damagable.TakeDamage(damageAmount);
                Debug.Log("Damage");
            }
        }

#if UNITY_EDITOR
        protected override void OnDrawGizmosSelected()
        {
            base.OnDrawGizmosSelected();
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _damageZone);
        }
#endif
    }
}