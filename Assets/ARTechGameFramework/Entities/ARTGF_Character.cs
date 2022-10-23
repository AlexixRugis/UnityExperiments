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

        public ARTGF_Character GetNearest(Predicate<ARTGF_Character> match, float visionDistance)
        {
            float minSqrDistance = float.PositiveInfinity;
            ARTGF_Character entity = null;

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

        public bool CanSee(ARTGF_Character character, float maxDistance)
        {
            if (Physics.Linecast(transform.position, character.transform.position, out RaycastHit hit, obstaclesMask))
            {
                if (hit.transform.GetComponent<ARTGF_Character>() == character && hit.distance < maxDistance) return true;
            }

            return false;
        }

        private IEnumerable<ARTGF_Character> GetNearbyCharacters(Predicate<ARTGF_Character> match, float visionDistance)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, visionDistance, charactersMask);
            var entities = new List<ARTGF_Character>(colliders.Length);
            foreach (var collider in colliders)
            {
                var entity = collider.GetComponent<ARTGF_Character>();
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
            Vector3 direction = UnityEngine.Random.onUnitSphere * radius;
            Vector3 directionNormalized = direction.normalized;

            if (!Physics.SphereCast(transform.position, MovementController.AgentRadius, directionNormalized, out RaycastHit hit, radius, obstaclesMask))
            {
                Vector3 position = transform.position + direction;

                if (Area) position = Area.GetClosestPoint(position);

                if (Vector3.Distance(position, transform.position) > radius)
                {
                    return null;
                }

                return position;
            }

            return null;
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