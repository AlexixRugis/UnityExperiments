using UnityEngine;

namespace ARTech.GameFramework
{
    public interface IMovement
    {
        float Speed { get; set; }
        Vector3 CurrentVelocity { get; }
        bool TryMove(Vector3? position);
        bool Teleport(Vector3? position);
        void ClearPath();
        bool HasPath();
        float GetRemainingDistance();

        Vector3? GetRandomPositionAround(Vector3 center, float radius);
        Vector3? GetPositionFrom(Vector3 center, Vector3 from, float radius);
    }
}