using System;
using UnityEngine;

namespace ARTech.GameFramework
{
    [RequireComponent(typeof(Collider))]
    public class WeaponHitbox : MonoBehaviour
    {
        [SerializeField] private float damage;

        public Predicate<IDamageable> DamageablePredicate { get; set; } = c => true;

        private void Awake()
        {
            Collider collider = GetComponent<Collider>();
            collider.isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            IDamageable damageable = other.GetComponent<IDamageable>();

            if (damageable != null && DamageablePredicate.Invoke(damageable))
            {
                damageable.TakeDamage(damage);
            }
        }
    }
}