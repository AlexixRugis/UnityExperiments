using ARTech.GameFramework;
using System.Collections.Generic;
using UnityEngine;

namespace Mobs
{
    [RequireComponent(typeof(MeshRenderer))]
    public class ChainDamageEntity : MonoBehaviour
    {
        [SerializeField] private float _size;
        [SerializeField] private float _deathTime;
        [SerializeField] private LayerMask _findMask;

        public void Initialize(Vector3 from, Vector3 to, float maxRadius, int damage, int remainingDamagebles, List<ARTGF_IDamageable> damaged)
        {
            Vector3 delta = to - from;
            float distance = delta.magnitude;


            transform.localScale = new Vector3(1, 1, distance / _size);
            transform.LookAt(to);

            ARTGF_IDamageable damagable = FindNearestNonDamaged(to, damaged, maxRadius, out Vector3 position);
            Debug.Log(damagable);

            if (damagable != null)
            {

                damagable.TakeDamage(damage);
                damaged.Add(damagable);

                if (remainingDamagebles - 1 > 0)
                {
                    ChainDamageFactory.Spawn(this, to, position, maxRadius, damage, remainingDamagebles - 1, damaged);
                }
            }

            Destroy(gameObject, _deathTime);
        }

        private ARTGF_IDamageable FindNearestNonDamaged(Vector3 startPosition, List<ARTGF_IDamageable> damaged, float maxRadius, out Vector3 position)
        {
            Collider[] colliders = Physics.OverlapSphere(startPosition, maxRadius, _findMask);

            int nearestIndex = -1;
            float nearestDistance = int.MaxValue;

            for (int i = 0; i < colliders.Length; i++)
            {
                var damagable = colliders[i].GetComponent<ARTGF_IDamageable>();
                if (damagable != null && !damaged.Contains(damagable))
                {
                    float distance = Vector3.Distance(startPosition, colliders[i].transform.position);
                    if (distance < nearestDistance)
                    {
                        nearestDistance = distance;
                        nearestIndex = i;
                    }
                }
            }

            if (nearestIndex == -1)
            {
                position = Vector3.zero;
                return null;
            }

            position = colliders[nearestIndex].transform.position;
            return colliders[nearestIndex].GetComponent<ARTGF_IDamageable>();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireCube(transform.position, Vector3.one * _size * 2);
        }
    }
}