using UnityEngine;

namespace Mobs
{
    public abstract class Bullet : MonoBehaviour
    {
        [SerializeField] private int _damage;

        [SerializeField] private LayerMask _hitMask;
        [SerializeField] private float _hitCheckDistance;

        private bool _isInitialized = false;

        public LayerMask HitMask => _hitMask;
        public float HitCheckDistance => _hitCheckDistance;
        public int Damage => _damage;

        public Vector3 TargetPosition { get; private set; }
        public Vector3 StartPosition { get; private set; }
        public float MovementSpeed { get; private set; }

        public void Initialize(Vector3 startPosition, Vector3 targetPosition, float speed, float lifetime = 5f)
        {
            transform.position = startPosition;

            transform.LookAt(targetPosition);

            MovementSpeed = speed;

            _isInitialized = true;

            Destroy(gameObject, lifetime);
        }

        protected virtual void HandleUpdate()
        {
            HandleMovement();

            if (Physics.CheckSphere(transform.position, _hitCheckDistance, _hitMask))
            {
                HandleHit();
            }
        }

        protected virtual void HandleMovement()
        {
            transform.Translate(0, 0, MovementSpeed * Time.deltaTime);
        }

        protected virtual void HandleHit()
        {
            Debug.Log("Hit!");
            Destroy(gameObject);
        }

        private void Update()
        {
            if (_isInitialized)
                HandleUpdate();
        }

#if UNITY_EDITOR
        protected virtual void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;

            Gizmos.DrawWireSphere(transform.position, _hitCheckDistance);
        }
#endif
    }
}