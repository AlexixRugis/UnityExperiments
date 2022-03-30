using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Mobs
{
    public class Mob : MonoBehaviour, IDamagable
    {
        public enum AIState
        {
            Idle,
            Attack
        }

        [Header("Player")]
        public Transform Player;

        [Header("Health")]
        public int MaxHealth;
        [Header("Heal")]
        public int HealAmount;
        public float HealInterval;
        public bool HealFullOnStartPatrol;
        public bool HealOnChase;

        [Header("Chase settings")]
        public float ChaseTimeSeconds;
        public float AgroRadius;

        public int Health { get; private set; }
        public AIState StateAI { get; private set; }

        private float _chaseStartTime;
        private float _lastHealTime;

        #region Health

        public void TakeDamage(int amount)
        {
            if (amount < 0) throw new System.Exception("amount must be greater o equal to zero");

            Health = Mathf.Max(0, Health - amount);

            OnDamage(amount);

            if (Health == 0)
            {
                Die();
            }
        }

        public void Heal(int amount)
        {
            if (amount < 0) throw new System.Exception("amount must be greater o equal to zero");

            Health = Mathf.Min(MaxHealth, Health + amount);

            OnHeal(amount);
        }

        protected virtual void OnDamage(int amount)
        {

        }

        protected virtual void OnHeal(int amount)
        {

        }

        protected virtual void Die()
        {

        }

        #endregion

        #region AI

        private Vector3 LocatePlayer()
        {
            if (!Player) throw new System.Exception("Player instance mustn't be null!");

            return Player.position;
        }

        public void ProcessAI()
        {
            if (!Player) return;

            Vector3 playerPosition = LocatePlayer();

            float distanceToPlayer = (playerPosition - transform.position).magnitude;
            
            if (StateAI == AIState.Idle)
            {
                ProcessIdle();
                if (distanceToPlayer < AgroRadius)
                    StartAttack();
            }
            else if (StateAI == AIState.Attack)
            {
                ProcessAttack();
                if (distanceToPlayer < AgroRadius)
                    _chaseStartTime = Time.time;
                else if (Time.time - _chaseStartTime > ChaseTimeSeconds)
                    StartIdle();
            }

        }

        protected virtual void ProcessAttack()
        {
            
        }

        protected virtual void ProcessIdle()
        {

        }

        public virtual void StartIdle()
        {
            StateAI = AIState.Idle;

            if (HealFullOnStartPatrol)
                Heal(MaxHealth);
        }

        public virtual void StartAttack()
        {
            StateAI = AIState.Attack;
        }

        #endregion

        #region Heal

        private void ProcessHeal()
        {
            if ((Time.time - _lastHealTime) > HealInterval && (StateAI != AIState.Attack || HealOnChase))
            {
                _lastHealTime = Time.time;
                Heal(HealAmount);
            }
        }

        #endregion

        protected virtual void Awake()
        {
            Health = MaxHealth;
        }

        protected virtual void Start()
        {
            StartIdle();
        }

        protected virtual void Update()
        {
            if (Health != 0)
            {
                ProcessAI();
                ProcessHeal();
            }
        }

#if UNITY_EDITOR
        protected virtual void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, AgroRadius);
        }
#endif
    }
}