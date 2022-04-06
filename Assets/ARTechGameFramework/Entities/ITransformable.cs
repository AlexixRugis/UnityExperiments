using UnityEngine;

namespace ARTech.GameFramework
{
    public interface ITransformable
    {
        Vector3 Position { get; set; }
        Quaternion Rotation { get; set; }
    }
}