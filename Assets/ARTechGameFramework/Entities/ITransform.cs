using UnityEngine;

namespace ARTech.GameFramework
{
    public interface ITransform
    {
        string Name { get; set; }
        Vector3 Position { get; set; }
        Quaternion Rotation { get; set; }
        float SpawnTime { get; }
        void Remove();
        bool IsRemoved { get; }
    }
}