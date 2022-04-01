using System;
using System.Collections.Generic;
using UnityEngine;

namespace ARTech.GameFramework
{
    public interface IEntity
    {
        string Name { get; set; }
        Vector3 Location { get; set; }
        Quaternion Rotation { get; set; }
        float GetDistanceToGround();
        bool IsOnGround { get; }
        IEnumerable<IEntity> GetNearbyEntities(float radius, Type[] entityTypes);
        IEntity GetNearest(float radius, Type[] entityTypes);
        void Remove();
        float DieTime { get; }
        bool IsRemoved { get; }
    }
}