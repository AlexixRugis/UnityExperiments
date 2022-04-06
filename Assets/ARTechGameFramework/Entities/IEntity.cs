using System;
using System.Collections.Generic;

namespace ARTech.GameFramework
{
    public interface IEntity : ITransformable
    {
        string Name { get; set; }
        IEnumerable<IEntity> GetNearbyEntities(float radius, Type[] entityTypes);
        IEntity GetNearest(float radius, Type[] entityTypes);
        void Remove();
        bool IsRemoved { get; }
    }
}