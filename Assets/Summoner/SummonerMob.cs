using ARTech.GameFramework;
using ARTech.GameFramework.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mobs
{
    [RequireComponent(typeof(SummonerAttackHandler))]
    public sealed class SummonerMob : Character
    {
        [Header("Anger")]
        [SerializeField] private float angerRadius;
        [SerializeField] private float targetLostTime;
        [Header("Attack")]
        [SerializeField] private float attackSpeed;
        [Header("Strafe")]
        [SerializeField] private float strafeRadius;
        [SerializeField] private float strafeSpeed;
        [SerializeField] private float strafeTime;
        [Header("Wander")]
        [SerializeField] private float minPatrolDistance;
        [SerializeField] private float maxPatrolDistance;
        [SerializeField] private float patrolSpeed;
        [SerializeField] private float minPatrolDuration;
        [SerializeField] private float maxPatrolDuration;
        [Header("Returning to area")]
        [SerializeField] private float distanceToArea;
        [SerializeField] private float teleportDuration;
        [SerializeField] private GameObject teleportGFX;

        private AIStateMachine _stateMachine;

        protected override void HandleSpawn()
        {
            base.HandleSpawn();
            SetupAI();
        }

        private void SetupAI()
        {
            _stateMachine = new AIStateMachine();
            _stateMachine.AddSensorTask(new AngerTypesSensor(this, c => c is Player, angerRadius, targetLostTime));

            _stateMachine.AddState(new RangedAttackNode(this, GetComponent<SummonerAttackHandler>(), attackSpeed));
            _stateMachine.AddState(new StrafeState(this, strafeRadius, strafeSpeed, strafeTime));
            _stateMachine.AddState(new TeleportToAreaNode(this, distanceToArea, teleportDuration, teleportGFX));
            _stateMachine.AddState(new WanderAroundNode(this, minPatrolDistance, maxPatrolDistance, patrolSpeed, minPatrolDuration, maxPatrolDuration));
        }

        protected override void HandleLifeUpdate()
        {
            base.HandleLifeUpdate();
            _stateMachine.Tick();
        }
    }
}