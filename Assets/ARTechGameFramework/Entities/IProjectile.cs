using System;
using UnityEngine;

namespace ARTech.GameFramework
{
    public interface IProjectile : ITransformableObject
    {
        void Launch(Vector3 direction);
        IEntity Shooter { get; set; }
        float Speed { get; set; }
        float Damage { get; set; }
        Predicate<IHealth> DamageableTypesPredicate { get; set; }
        float Lifetime { get; set; }
        float RemainingLifetime { get; }
        bool Launched { get; }
    }
}