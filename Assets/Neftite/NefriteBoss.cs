using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mobs
{
    public sealed class NefriteBoss : Boss
    {

        [Header("Stage settings")]
        public float BetweenAttackTime = 0.5f;
        [Range(0f, 1f)]
        public float KulakEndHealthPercent = 1f;
        public int MinAttackRepeats = 3;
        public int MaxAttackRepeats = 7;

        [Header("Attack spheres")]
        public float CloseZoneRadius = 1f;
        public float MiddleZoneRadius = 5f;
        public float AwareZoneRadius = 10f;

        private IBossStage[] _stages;

        private IBossStage _currentStage;

        private bool _isAttacking = false;
        private int _attacksAmount = 0;
        private int _attacksDone = 0;

        protected override void Start()
        {
            _stages = new IBossStage[] { GetComponentInChildren<NefriteBossFistStage>(),
                GetComponentInChildren<NefriteBossCreationStage>(),
                GetComponentInChildren<NefriteBossTruthStage>(),
                GetComponentInChildren<NefriteBossStrengthStage>() };
        }

        protected override void ProcessAttack()
        {
            base.ProcessAttack();

            if (!_isAttacking)
            {
                StartCoroutine(ProcessCurrentOrNewAttack());
            }   
        }

        private IEnumerator TryGoNextStage()
        {
            if (_attacksAmount <= _attacksDone || _currentStage == null)
            {
                _attacksAmount = GetAttackRepeats();
                _attacksDone = 0;

                if (_currentStage != null) { 
                    yield return _currentStage.EndStage();
                }

                NextStage();

                yield return _currentStage.StartStage();
            }
            else
            {
                yield return null;
            }
        }

        private void NextStage()
        {
            if (_currentStage == null)
            {
                _currentStage = _stages[0];
            }
            if (_currentStage != _stages[0] || (float)Health / maxHealth < KulakEndHealthPercent)
            {
                _currentStage = _stages[UnityEngine.Random.Range(1, _stages.Length)];
                //_currentStage = _stages[2];
            }
        }

        protected override void ProcessIdle()
        {
            base.ProcessIdle();
        }

        protected override void Die()
        {
            base.Die();
            StopAllCoroutines();
        }

        public Transform GetPlayer()
        {
            return Player;
        }

        public int GetPlayerZone()
        {
            float distance = Vector3.Distance(Player.position, transform.position);

            int zone = -1;
            if (distance < CloseZoneRadius)
            {
                zone = 1;
            }
            else if (distance < MiddleZoneRadius)
            {
                zone = 2;
            }
            else if (distance < AwareZoneRadius)
            {
                zone = 3;
            }

            return zone;
        }

        protected override void OnDamage(float amount)
        {
            base.OnHeal(amount);
        }

        private int GetAttackRepeats()
        {
            return UnityEngine.Random.Range(MinAttackRepeats, MaxAttackRepeats);
        }

        private IEnumerator ProcessCurrentOrNewAttack()
        {
            _isAttacking = true;

            yield return TryGoNextStage();

            Debug.Log(_currentStage.GetType());

            _attacksDone++;

            yield return new WaitForSeconds(BetweenAttackTime);

            yield return _currentStage.ProcessStage();

            _isAttacking = false;
        }

#if UNITY_EDITOR
        protected override void OnDrawGizmosSelected()
        {
            base.OnDrawGizmosSelected();

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, CloseZoneRadius);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, MiddleZoneRadius);
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, AwareZoneRadius);
        }
#endif
    }
}
