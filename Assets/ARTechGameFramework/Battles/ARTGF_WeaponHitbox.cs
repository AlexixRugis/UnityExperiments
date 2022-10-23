using System;
using UnityEngine;

namespace ARTech.GameFramework
{
    [RequireComponent(typeof(Collider))]
    public class ARTGF_WeaponHitbox : MonoBehaviour
    {
        [SerializeField] private float damage;

        public Predicate<ARTGF_IDamageable> DamageablePredicate { get; set; } = c => true;

        private void Awake()
        {
            Collider collider = GetComponent<Collider>();
            collider.isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            ARTGF_IDamageable damageable = other.GetComponent<ARTGF_IDamageable>();

            if (damageable != null && DamageablePredicate.Invoke(damageable))
            {
                damageable.TakeDamage(damage);
            }
        }
    }
}