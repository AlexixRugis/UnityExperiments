using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ARTech.GameFramework
{
    public class ARTGF_Character : MonoBehaviour, ARTGF_IDamageable
    {
        [Serializable]
        public class CharacterEvent : UnityEvent<ARTGF_Character> { }

        [SerializeField] private ARTGF_Stat maxHealth;
        [SerializeField] private ARTGF_Stat protection;
        [SerializeField] private bool isImmortal;

        [Header("Detection")]
        [SerializeField] private LayerMask charactersMask;
        [SerializeField] private LayerMask obstaclesMask;

        public CharacterEvent OnDead = new CharacterEvent();
        public CharacterEvent OnHealthChanged = new CharacterEvent();

        public bool IsAlive { get; protected set; }
        public float CurrentHealth { get; protected set; }
        public bool IsImmortal { get => isImmortal; set => isImmortal = value; }

        public ARTGF_Stat MaxHealth => maxHealth;
        public ARTGF_Stat Protection => protection;

        public ARTGF_Character BattleTarget { get; set; }

        public ARTGF_SpawnArea Area { get; set; }
        public ARTGF_IMovement MovementController { get; private set; }

        protected virtual void Start()
        {
            MovementController = GetComponent<ARTGF_IMovement>();

            HandleSpawn();
        }

        protected virtual void Update()
        {
            if (IsAlive)
            {
                HandleLifeUpdate();
            }
        }

        public void Heal(float amount)
        {
            if (!IsAlive) return;

            float lastHealth = CurrentHealth;
            CurrentHealth += amount;

            if (CurrentHealth >= maxHealth.Value)
            {
                CurrentHealth = maxHealth.Value;
            }

            HandleHealthChange(lastHealth);
        }

        public void TakeDamage(float amount)
        {
            if (!IsAlive || isImmortal) return;

            amount = Mathf.Clamp(amount - protection.Value, Mathf.CeilToInt((float)amount * 0.2f), amount);

            float lastHealth = CurrentHealth;
            CurrentHealth -= amount;

            if (CurrentHealth <= 0)
            {
                CurrentHealth = 0;
                HandleDie();
            }

            HandleHealthChange(lastHealth);
        }

        protected virtual void HandleLifeUpdate()
        {
        }

        protected virtual void HandleHealthChange(float lastHealth)
        {
            OnHealthChanged?.Invoke(this);
        }

        protected virtual void HandleSpawn()
        {
            IsAlive = true;
            CurrentHealth = maxHealth.Value;
        }

        protected virtual void HandleDie()
        {
            IsAlive = false;
            OnDead?.Invoke(this);
            Destroy(gameObject);
        }
    }
}