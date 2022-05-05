using UnityEngine;

namespace ARTech.GameFramework
{
    public static class PositionUtils
    {
        public static Vector3? GetPositionFrom(Vector3 position, Vector3 from, float agentRadius, float radius, LayerMask obstaclesMask)
        {
            Vector3 directionNormalized = (position - from).normalized;

            if (!Physics.SphereCast(position, agentRadius, directionNormalized, out RaycastHit hit, radius, obstaclesMask))
            {
                return position + directionNormalized * radius;
            }
            else
            {
                float distance = hit.distance - agentRadius;
                if (distance <= 0)
                {
                    return position;
                }

                return position + directionNormalized * (hit.distance - agentRadius);
            }
        }

        public static Vector3? GetRandomPositionAround(SpawnArea area, Vector3 position, float agentRadius, float radius)
        {
            return area?.GetRandomPointAround(position, radius, agentRadius);
        }
    }
}