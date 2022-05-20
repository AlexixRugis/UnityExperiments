using ARTech.GameFramework;
using ARTech.GameFramework.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mobs
{
    [RequireComponent(typeof(LeafSpiritAttackHandler))]
    public sealed class LeafSpirit : Character
    {
        [Header("Anger")]
        [SerializeField] private float angerRadius;
        [SerializeField] private float attackMovementSpeed;
        [SerializeField] private float targetLostTime;
        [Header("Strafe")]
        [SerializeField] private float strafeDistance;
        [SerializeField] private float strafeSpeed;
        [Header("Wander")]
        [SerializeField] private float minWanderDistance;
        [SerializeField] private float maxWanderDistance;
        [SerializeField] private float wanderSpeed;
        [SerializeField] private float minPatrolDuration;
        [SerializeField] private float maxPatrolDuration;
        [Header("Teleport to area")]
        [SerializeField] private float distanceToTeleport;
        [SerializeField] private float teleportDuration;
        [SerializeField] private GameObject teleportGFX;

        private AIStateMachine _stateMachine = new AIStateMachine();
        private LeafSpiritAttackHandler _attackHandler;

        protected override void HandleSpawn()
        {
            base.HandleSpawn();
            _attackHandler = GetComponent<LeafSpiritAttackHandler>();
            SetupAI();
        }

        private void SetupAI()
        {
            _stateMachine.AddState(new MeleeAttackNode(this, _attackHandler, attackMovementSpeed));
            _stateMachine.AddState(new StrafeState(this, strafeDistance, strafeSpeed, 1.5f));
            _stateMachine.AddState(new TeleportToAreaNode(this, distanceToTeleport, teleportDuration, teleportGFX));
            _stateMachine.AddState(new WanderAroundNode(this, minWanderDistance, maxWanderDistance, wanderSpeed, minPatrolDuration, maxPatrolDuration));
            _stateMachine.AddSensorTask(new AngerTypesSensor(this, c => c is Player, angerRadius, targetLostTime));
        }

        protected override void HandleLifeUpdate()
        {
            base.HandleLifeUpdate();
            _stateMachine.Tick();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, angerRadius);
        }
    }
}