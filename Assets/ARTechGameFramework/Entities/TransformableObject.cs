using UnityEngine;

namespace ARTech.GameFramework
{
    public class TransformableObject : MonoBehaviour, ITransformableObject
    {
        public string Name { get => name; set => name = value; }
        public Vector3 Position { get => !IsRemoved ? transform.position : Vector3.zero; set { if (!IsRemoved) transform.position = value; } }
        public Quaternion Rotation { get => !IsRemoved ? transform.rotation : Quaternion.identity; set { if (!IsRemoved) transform.rotation = value; } }

        public bool IsRemoved { get; protected set; }

        public virtual void Remove()
        {
            if (IsRemoved) return;
            IsRemoved = true;
            Destroy(gameObject);
        }
    }
}