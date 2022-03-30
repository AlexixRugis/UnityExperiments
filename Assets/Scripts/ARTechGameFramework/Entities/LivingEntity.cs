using System;
using UnityEngine;

namespace ARTech.GameFramework
{
    public abstract class LivingEntity : Entity, IDamageable
    {
        [SerializeField] private bool _isImmortal;

        [SerializeField] private Vector3 _eyeOffset;
        [SerializeField] private float _visionDistance;
        [SerializeField] private float _loseTargetDuration;
        [SerializeField] private float _health;
        [SerializeField] private float _maxHealth;

        private LivingEntity _target;
        private float _lastSeeTime;

        private Entity _lastDamager;

        protected virtual void Update()
        {
            LivingEntity target = GetTarget();
            if (target)
            {
                if (CanSee(target))
                {
                    _lastSeeTime = Time.time;
                }

                if (Time.time - _lastSeeTime > _loseTargetDuration)
                {
                    SetTarget(null);
                }
            }
        }

        public float GetHealth() => _health;
        public float GetMaxHealth() => _maxHealth;
        public void ResetHealth() => _health = _maxHealth;
        public void SetHealth(float health) => _health = Mathf.Clamp(health, 0, _maxHealth);
        public void SetMaxHealth(float maxHealth)
        {
            _maxHealth = maxHealth;
            _health = Mathf.Clamp(_health, 0, _maxHealth);
        }
        public void TakeDamage(float amount)
        {
            TakeDamage(amount, null);
        }

        public void TakeDamage(float amount, Entity source)
        {
            if (amount < 0) throw new InvalidOperationException(nameof(amount));

            if (_isImmortal || IsRemoved()) return;

            _health = Mathf.Clamp(_health - amount, 0, _maxHealth);
            _lastDamager = source;

            if (_health == 0)
            {
                Remove();
            }
        }

        public Entity GetLastDamager() => _lastDamager;
        public bool IsImmortal() => _isImmortal;
        public Vector3 GetEyeOffset() => _eyeOffset;
        public Vector3 GetEyeLocation() => transform.TransformPoint(_eyeOffset);
        public LivingEntity GetTarget() => _target;
        public float GetTargetLastSeeTime() => _lastSeeTime;
        public void SetTarget(LivingEntity target) {
            _target = target;
        }
        public bool CanSee(Entity target)
        {
            if (!target) throw new InvalidOperationException($"Entity {GetName()} has no target!");

            if (Physics.Linecast(GetEyeLocation(), target.GetLocation(), out RaycastHit hit))
            {
                if (hit.transform == target.transform && hit.distance < _visionDistance) return true;
            }

            return false;
        }

        protected override void OnDrawGizmosSelected()
        {
            base.OnDrawGizmosSelected();

            if (!DebugEnabled) return;
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(GetEyeLocation(), _visionDistance);
        }
    }
}