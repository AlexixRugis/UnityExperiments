using UnityEngine;

namespace ARTech.GameFramework
{
    [RequireComponent(typeof(ITransform))]
    public sealed class DestroyEntity : MonoBehaviour
    {
        [Tooltip("Infinity if 0")]
        [SerializeField] private float _lifetime = 5f;

        private ITransform _transform;

        private void Awake()
        {
            _transform = GetComponent<ITransform>();    
        }

        private void Update()
        {
            if (_lifetime > 0f && Time.time - _transform.SpawnTime > _lifetime)
            {
                _transform.Remove();
                Destroy(this);
            }
        }
    }
}