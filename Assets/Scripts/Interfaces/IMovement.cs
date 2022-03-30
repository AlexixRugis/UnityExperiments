using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mobs
{
    public interface IMovement
    {
        bool IsRotatingToMovement { get; set; }
        float Speed { get; set; }
        bool ReachedDestination { get; }
        void SetDestination(Vector3 position);
        void Stop();
    }
}