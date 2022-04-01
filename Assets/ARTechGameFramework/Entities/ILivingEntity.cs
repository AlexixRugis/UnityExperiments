using UnityEngine;

namespace ARTech.GameFramework
{
    public interface ILivingEntity : IEntity, IDamageable
    {
        ILivingEntity Target { get; set; }
        float LastTargetSeeTime { get; }
        bool CanSee(ILivingEntity target);
        Vector3 EyeOffset { get; }
        Vector3 EyeLocation { get; }
        float VisionDistance { get; }
    }
}