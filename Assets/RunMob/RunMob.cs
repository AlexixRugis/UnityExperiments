using ARTech.GameFramework;
using ARTech.GameFramework.AI;
using UnityEngine;

namespace Mobs
{
    [RequireComponent(typeof(RunMobMeleeHandler))]
    public sealed class RunMob : ARTGF_Character
    {
        [Header("Anger")]
        [SerializeField] private float angerRadius;
        [SerializeField] private float targetLostTime;
        [Header("Patrol")]
        [SerializeField] private float minPatrolDistance;
        [SerializeField] private float maxPatrolDistance;
        [SerializeField] private float patrolSpeed;
        [SerializeField] private float minPatrolDuration;
        [SerializeField] private float maxPatrolDuration;
        [Header("Strafe")]
        [SerializeField] private float strafeRadius;
        [SerializeField] private float strafeSpeed;
        [Header("Attacks")]
        [SerializeField] private float meleeAttackSpeed;
        [SerializeField] private float runAttackSpeed;
        [Header("Return area")]
        [SerializeField] private float teleportDistance;
        [SerializeField] private float teleportDuration;
        [SerializeField] private GameObject teleportGfx;

        private ARTGF_AIStateMachine _stateMachine;

        protected override void HandleLifeUpdate()
        {
            base.HandleLifeUpdate();
            _stateMachine.Tick();
        }

        protected override void HandleSpawn()
        {
            base.HandleSpawn();
            SetupAI();
        }
        private void SetupAI()
        {
            _stateMachine = new ARTGF_AIStateMachine();

            _stateMachine.AddSensorTask(new ARTGF_AngerTypesSensor(this, c => c is ARTGF_Player, angerRadius, targetLostTime));

            _stateMachine.AddState(new ARTGF_RangedAttackNode(this, GetComponent<RunMobRangedHandler>(), runAttackSpeed));
            _stateMachine.AddState(new ARTGF_MeleeAttackNode(this, GetComponent<RunMobMeleeHandler>(), meleeAttackSpeed));
            _stateMachine.AddState(new ARTGF_StrafeState(this, strafeRadius, strafeSpeed, 2f));
            _stateMachine.AddState(new ARTGF_TeleportToAreaNode(this, teleportDistance, teleportDuration, teleportGfx));
            _stateMachine.AddState(new ARTGF_WanderAroundNode(this, minPatrolDistance, maxPatrolDistance, patrolSpeed, minPatrolDuration, maxPatrolDuration));
        }
    }
}