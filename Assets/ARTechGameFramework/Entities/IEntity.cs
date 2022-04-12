using System;
using System.Collections.Generic;

namespace ARTech.GameFramework
{
    public interface IEntity : ITransformableObject
    {
        IEnumerable<IEntity> GetNearbyEntities(float radius, Predicate<IEntity> match);
        IEntity GetNearest(float radius, Predicate<IEntity> match);
    }
}