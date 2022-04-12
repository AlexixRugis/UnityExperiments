using UnityEngine;

namespace ARTech.GameFramework
{
    public interface IArea
    {
        float GetDistance(Vector3 from);
        Vector3? GetRandomPointAround(Vector3 center, float maxDistance, float agentRadius);
        Vector3? GetRandomPointIn();
    }
}