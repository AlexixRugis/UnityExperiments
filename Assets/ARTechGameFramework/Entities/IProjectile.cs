using UnityEngine;

namespace ARTech.GameFramework
{
    public interface IProjectile : IEntity
    {
        void Launch(Vector3 direction);
        IEntity Shooter { get; set; }
        float Speed { get; set; }
        float Damage { get; set; }
        float Lifetime { get; set; }
        float RemainingLifetime { get; }
        bool Launched { get; }
    }
}