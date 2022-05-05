using System;
using UnityEngine;

namespace ARTech.GameFramework
{
    public interface IProjectile : ITransform
    {
        void Launch(Vector3 direction);
        ICharacter Shooter { get; set; }
        float Speed { get; set; }
        float Damage { get; set; }
        Predicate<IDamageable> DamageableTypesPredicate { get; set; }
        bool Launched { get; }
    }
}