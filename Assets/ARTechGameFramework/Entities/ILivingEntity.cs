using UnityEngine;

namespace ARTech.GameFramework
{
    public interface ILivingEntity : IEntity, IHealth
    {
        IHealth Target { get; set; }
        float LastTargetSeeTime { get; }
        bool CanSee(ITransformableObject target);
        Vector3 EyeOffset { get; }
        Vector3 EyeLocation { get; }
        float VisionDistance { get; }
    }
}