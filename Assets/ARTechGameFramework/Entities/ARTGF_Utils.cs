using System.Collections.Generic;
using System;
using UnityEngine;

namespace ARTech.GameFramework
{
    public static class ARTGF_Utils
    {
        public static ARTGF_Character GetNearest(Vector3 from, float visionDistance, Predicate<ARTGF_Character> match)
        {
            float minSqrDistance = float.PositiveInfinity;
            ARTGF_Character entity = null;

            foreach (var e in GetNearbyCharacters(from, match, visionDistance))
            {
                float sqrDistance = (from - e.transform.position).sqrMagnitude;
                if (sqrDistance < minSqrDistance)
                {
                    minSqrDistance = sqrDistance;
                    entity = e;
                }

                break;
            }

            return entity;
        }

        public static bool CanSee(Vector3 from, ARTGF_Character character, float maxDistance, int obstacleMask = Physics.DefaultRaycastLayers)
        {
            if (Physics.Linecast(from, character.transform.position, out RaycastHit hit, obstacleMask))
            {
                if (hit.transform.GetComponent<ARTGF_Character>() == character && hit.distance < maxDistance) return true;
            }

            return false;
        }

        private static IEnumerable<ARTGF_Character> GetNearbyCharacters(Vector3 from, Predicate<ARTGF_Character> match, float visionDistance, int charactersMask = Physics.DefaultRaycastLayers)
        {
            Collider[] colliders = Physics.OverlapSphere(from, visionDistance, charactersMask);
            var entities = new List<ARTGF_Character>(colliders.Length);
            foreach (var collider in colliders)
            {
                var entity = collider.GetComponent<ARTGF_Character>();
                if (entity != null && match.Invoke(entity))
                {
                    entities.Add(entity);
                }
            }

            return entities;
        }

        public static Vector3? GetPositionFrom(Vector3 from, Vector3 other, float agentRadius, float maxDistance, int obstaclesMask = Physics.DefaultRaycastLayers)
        {
            Vector3 directionNormalized = (from - other).normalized;

            if (!Physics.SphereCast(from, agentRadius, directionNormalized, out RaycastHit hit, maxDistance, obstaclesMask))
            {
                return from + directionNormalized * maxDistance;
            }
            else
            {
                float distance = hit.distance - agentRadius;
                if (distance <= 0)
                {
                    return from;
                }

                return from + directionNormalized * (hit.distance - agentRadius);
            }
        }

        public static Vector3? GetRandomPositionAround(Vector3 from, float agentRadius, float maxDistance, int obstaclesMask = Physics2D.DefaultRaycastLayers)
        {
            Vector3 direction = UnityEngine.Random.onUnitSphere * maxDistance;
            Vector3 directionNormalized = direction.normalized;

            if (!Physics.SphereCast(from, agentRadius, directionNormalized, out RaycastHit hit, maxDistance, obstaclesMask))
            {
                Vector3 position = from + direction;
                return position;
            }

            return null;
        }
    }
}
