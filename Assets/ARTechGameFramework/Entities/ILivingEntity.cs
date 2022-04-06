using UnityEngine;

namespace ARTech.GameFramework
{
    public interface ILivingEntity : IEntity, IDamageable
    {
        IDamageable Target { get; set; }
        float LastTargetSeeTime { get; }
        bool CanSee(ITransformable target);
        Vector3 EyeOffset { get; }
        Vector3 EyeLocation { get; }
        float VisionDistance { get; }
    }
}