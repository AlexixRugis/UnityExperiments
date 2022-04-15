using UnityEngine;

namespace ARTech.GameFramework
{
    public class TransformableObject : MonoBehaviour, ITransform
    {
        public string Name { get => name; set => name = value; }
        public Vector3 Position { get => !IsRemoved ? transform.position : Vector3.zero; set { if (!IsRemoved) transform.position = value; } }
        public Quaternion Rotation { get => !IsRemoved ? transform.rotation : Quaternion.identity; set { if (!IsRemoved) transform.rotation = value; } }

        public float SpawnTime { get; private set; }
        public bool IsRemoved { get; protected set; }

        protected virtual void Start()
        {
            SpawnTime = Time.time;
        }

        public virtual void Remove()
        {
            if (IsRemoved) return;
            IsRemoved = true;
            Destroy(gameObject);
        }

        protected virtual void OnDestroy()
        {
            IsRemoved = true;
        }
    }
}