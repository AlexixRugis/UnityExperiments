using System.Collections.Generic;
using UnityEngine;

namespace ARTech.GameFramework
{
    public interface IArea
    {
        IEnumerable<Vector3> Spawnpoints { get; }
        IEnumerable<INPC> NPCs { get; }
        int SpawnDuration { get; set; }
        int MaxSpawnCount { get; set; }

        float GetDistance(Vector3 from);
        Vector3? GetRandomPointAround(Vector3 center, float maxDistance, float agentRadius);
        Vector3? GetRandomPointIn();
    }
}