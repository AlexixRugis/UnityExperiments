using UnityEngine;

namespace ARTech.GameFramework
{
    public interface ARTGF_IMovement
    {
        Vector3? FocusPoint { get; set; }
        float AgentRadius { get; }
        ARTGF_Character Character { get; }
        float Speed { get; set; }
        Vector3 CurrentVelocity { get; }
        Vector3 DestinationPoint { get; }

        bool TryMove(Vector3? position);
        bool Teleport(Vector3? position);
        void ClearPath();
        bool HasPath();
        float GetRemainingDistance();
    }
}