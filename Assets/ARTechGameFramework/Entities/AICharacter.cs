using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ARTech.GameFramework
{
    public class AICharacter : MonoBehaviour, IDamageable
    {
        [Serializable]
        public class CharacterEvent : UnityEvent<AICharacter> { }

        [SerializeField] private Stat maxHealth;
        [SerializeField] private Stat protection;

        [Header("Detection")]
        [SerializeField] private LayerMask charactersMask;
        [SerializeField] private LayerMask obstaclesMask;

        public CharacterEvent OnDead = new CharacterEvent();
        public CharacterEvent OnHealthChanged = new CharacterEvent();

        public bool IsAlive { get; protected set; }
        public float CurrentHealth { get; protected set; }

        public Stat MaxHealth => maxHealth;
        public Stat Protection => protection;

        public AICharacter BattleTarget { get; set; }

        public SpawnArea Area { get; set; }
        public IMovement MovementController { get; private set; }

        protected virtual void Start()
        {
            MovementController = GetComponent<IMovement>();

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
            if (!IsAlive) return;

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

        public AICharacter GetNearest(Predicate<AICharacter> match, float visionDistance)
        {
            float minSqrDistance = float.PositiveInfinity;
            AICharacter entity = null;

            foreach (var e in GetNearbyCharacters(match, visionDistance))
            {
                float sqrDistance = (transform.position - e.transform.position).sqrMagnitude;
                if (sqrDistance < minSqrDistance)
                {
                    minSqrDistance = sqrDistance;
                    entity = e;
                }

                break;
            }

            return entity;
        }

        public bool CanSee(AICharacter character, float maxDistance)
        {
            if (Physics.Linecast(transform.position, character.transform.position, out RaycastHit hit, obstaclesMask))
            {
                if (hit.transform.GetComponent<AICharacter>() == character && hit.distance < maxDistance) return true;
            }

            return false;
        }

        private IEnumerable<AICharacter> GetNearbyCharacters(Predicate<AICharacter> match, float visionDistance)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, visionDistance, charactersMask);
            var entities = new List<AICharacter>(colliders.Length);
            foreach (var collider in colliders)
            {
                var entity = collider.GetComponent<AICharacter>();
                if (entity != null && match.Invoke(entity))
                {
                    entities.Add(entity);
                }
            }

            return entities;
        }

        public Vector3? GetPositionFrom(Vector3 from, float radius)
        {
            Vector3 directionNormalized = (transform.position - from).normalized;

            if (!Physics.SphereCast(transform.position, MovementController.AgentRadius, directionNormalized, out RaycastHit hit, radius, obstaclesMask))
            {
                return transform.position + directionNormalized * radius;
            }
            else
            {
                float distance = hit.distance - MovementController.AgentRadius;
                if (distance <= 0)
                {
                    return transform.position;
                }

                return transform.position + directionNormalized * (hit.distance - MovementController.AgentRadius);
            }
        }

        public Vector3? GetRandomPositionAround(float radius)
        {
            return Area?.GetRandomPointAround(transform.position, radius, MovementController.AgentRadius);
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