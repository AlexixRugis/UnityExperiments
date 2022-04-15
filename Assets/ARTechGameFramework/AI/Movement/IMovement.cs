using UnityEngine;

namespace ARTech.GameFramework
{
    public interface IMovement
    {
        ICharacter Character { get; }
        float Speed { get; set; }
        Vector3 CurrentVelocity { get; }

        bool TryMove(Vector3? position);
        bool Teleport(Vector3? position);
        void ClearPath();
        bool HasPath();
        float GetRemainingDistance();
    }
}