using System;
using UnityEngine;

namespace ARTech.GameFramework
{
    public abstract class LivingEntity : Entity, ILivingEntity
    {
        [Header("Health")]
        [SerializeField] private bool _isImmortal;
        [SerializeField] private float _health;
        [SerializeField] private float _maxHealth;

        [Header("Targeting")]
        [SerializeField] private Vector3 _eyeOffset;
        [SerializeField] private float _visionDistance;
        [SerializeField] private float _loseTargetDuration;

        private float _lastSeeTime;
        public IHealth Target { get; set; }
        public float Health { get => _health; set => _health = Mathf.Clamp(value, 0, _maxHealth); }
        public float MaxHealth { get => _maxHealth; set { _maxHealth = value; _health = Mathf.Clamp(_health, 0, _maxHealth); } }

        protected override void OnLifeUpdate()
        {
            if (Target != null)
            {
                if (CanSee(Target))
                {
                    _lastSeeTime = Time.time;
                }

                if (Time.time - _lastSeeTime > _loseTargetDuration)
                {
                    Target = null;
                }
            }
        }
        public void ResetHealth() => _health = _maxHealth;
        public void TakeDamage(float amount)
        {
            if (amount < 0) throw new InvalidOperationException(nameof(amount));

            if (_isImmortal || IsRemoved) return;

            Health = _health - amount;

            if (_health == 0)
            {
                Remove();
            }
        }
        public Vector3 EyeOffset => _eyeOffset;
        public Vector3 EyeLocation => transform.TransformPoint(_eyeOffset);
        public float VisionDistance => _visionDistance;
        public float LastTargetSeeTime => _lastSeeTime;
        public bool CanSee(ITransformableObject target)
        {
            if (Physics.Linecast(EyeLocation, target.Position, out RaycastHit hit))
            {
                if (hit.transform.GetComponent<ILivingEntity>() == target && hit.distance < _visionDistance) return true;
            }

            return false;
        }

        protected override void OnDrawGizmosSelected()
        {
            base.OnDrawGizmosSelected();

            if (!DebugEnabled) return;
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(EyeLocation, _visionDistance);
        }
    }
}