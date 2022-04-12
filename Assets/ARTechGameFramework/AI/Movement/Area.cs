using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARTech.GameFramework
{
    [RequireComponent(typeof(Collider))]
    public class Area : MonoBehaviour, IArea
    {
        [SerializeField] private LayerMask _obstacleMask;

        private Collider _collider;

        private void Awake()
        {
            _collider = GetComponent<Collider>();
        }

        public Vector3? GetRandomPointAround(Vector3 center, float maxDistance, float agentRadius)
        {
            Vector3 direction = Random.onUnitSphere * maxDistance;
            Vector3 directionNormalized = direction.normalized;

            if (!Physics.SphereCast(transform.position, agentRadius, directionNormalized, out RaycastHit hit, maxDistance, _obstacleMask))
            {
                Vector3 position = _collider.ClosestPoint(center + direction);

                if (Vector3.Distance(position, center) >  maxDistance)
                {
                    return null;
                }

                return position;
            }

            return null;
        }

        public float GetDistance(Vector3 from)
        {
            return Vector3.Distance(_collider.ClosestPoint(from), from);
        }

        public Vector3? GetRandomPointIn()
        {
            Vector3 position = _collider.ClosestPoint(
                new Vector3(
                    Random.Range(_collider.bounds.min.x, _collider.bounds.max.x),
                    Random.Range(_collider.bounds.min.y, _collider.bounds.max.y),
                    Random.Range(_collider.bounds.min.z, _collider.bounds.max.z)
                )
            );

            return position;
        }
    }
}