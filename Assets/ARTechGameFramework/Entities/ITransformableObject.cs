using UnityEngine;

namespace ARTech.GameFramework
{
    public interface ITransformableObject
    {
        string Name { get; set; }
        Vector3 Position { get; set; }
        Quaternion Rotation { get; set; }
        void Remove();
        bool IsRemoved { get; }
    }
}