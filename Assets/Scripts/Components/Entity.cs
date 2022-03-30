using System;
using UnityEngine;

namespace Mobs
{
    public class Entity : MonoBehaviour, IDamagable, IHealable
    {
        [SerializeField] private bool _isImmortal = false;
        [SerializeField] private AttackableType _type;
        public AttackableType Type => _type;

        public delegate void EntityHealthEvent(Entity entity);
        public event EntityHealthEvent OnHealthChanged;
        public event EntityHealthEvent OnDied;

        [SerializeField] private int _maxHealth;

        private int _currentHealth;

        public int MaxHealth => _maxHealth;
        public int Health => _currentHealth;

        protected virtual void Awake()
        {
            _currentHealth = _maxHealth;
        }

        public void TakeDamage(int amount)
        {
            if (_isImmortal) return;
            if (amount < 0) throw new InvalidOperationException();

            _currentHealth = Mathf.Max(0, _currentHealth - amount);

            OnHealthChanged?.Invoke(this);

            if (_currentHealth == 0)
            {
                OnDied?.Invoke(this);
                Die();
            }
        }

        public void Heal(int amount)
        {
            if (_isImmortal) return;
            if (amount < 0) throw new InvalidOperationException();

            _currentHealth = Mathf.Min(_currentHealth + amount, _maxHealth);

            OnHealthChanged?.Invoke(this);
        }

        protected virtual void Die()
        {
            if (_isImmortal) return;
            Destroy(gameObject);

            Debug.Log($"{name} died!");
        }
    }
}