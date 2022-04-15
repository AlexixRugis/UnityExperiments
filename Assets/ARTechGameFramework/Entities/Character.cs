using System;
using System.Collections.Generic;
using UnityEngine;

namespace ARTech.GameFramework
{
    public abstract class Character : TransformableObject, ICharacter
    {
        public readonly Vector3 DirectionDown = Vector3.down;

        [SerializeField] private bool _debug = true;

        [Header("Health")]
        [SerializeField] private bool _isImmortal;
        [SerializeField] private float _health;
        [SerializeField] private float _maxHealth;

        [Header("Vision")]
        [SerializeField] private float _visionDistance;

        public bool DebugEnabled => _debug;

        public float Health { get => _health; set => _health = Mathf.Clamp(value, 0, _maxHealth); }
        public float MaxHealth { get => _maxHealth; set { _maxHealth = value; _health = Mathf.Clamp(_health, 0, _maxHealth); } }

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

        public bool CanSee(ICharacter target)
        {
            if (Physics.Linecast(Position, target.Position, out RaycastHit hit))
            {
                if (hit.transform.GetComponent<ILivingEntity>() == target && hit.distance < _visionDistance) return true;
            }

            return false;
        }

        public ICharacter GetNearest(Predicate<ICharacter> match)
        {
            float minSqrDistance = float.PositiveInfinity;
            ICharacter entity = null;

            foreach (var e in GetNearbyCharacters(match))
            {
                float sqrDistance = (Position - e.Position).sqrMagnitude;
                if (sqrDistance < minSqrDistance)
                {
                    minSqrDistance = sqrDistance;
                    entity = e;
                }

                break;
            }

            return entity;
        }
        private IEnumerable<ICharacter> GetNearbyCharacters(Predicate<ICharacter> match)
        {
            Collider[] colliders = Physics.OverlapSphere(Position, _visionDistance);
            var entities = new List<ICharacter>(colliders.Length);
            foreach (var collider in colliders)
            {
                var entity = collider.GetComponent<ICharacter>();
                if (entity != null && match.Invoke(entity))
                {
                    entities.Add(entity);
                }
            }

            return entities;
        }

        protected virtual void OnDrawGizmosSelected()
        {
            if (!DebugEnabled) return;

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(Position, _visionDistance);
        }
    }
}