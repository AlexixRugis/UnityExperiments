using UnityEngine;

namespace Mobs
{
    public enum MovementType
    {
        Patrol,
        Chase
    }

    public interface IMovementComponent
    {
        MovementType MovementType { get; }

        void StartPatrol();
        void StartChase(Transform chase);
    }
}