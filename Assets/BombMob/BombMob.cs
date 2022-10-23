using ARTech.GameFramework;
using ARTech.GameFramework.AI;
using UnityEngine;

namespace Mobs
{
    [RequireComponent(typeof(BombMobAttackHandler))]
    public sealed class BombMob : ARTGF_Character
    {
        [Header("Anger")]
        [SerializeField] private float angerRadius;
        [SerializeField] private float targetLostTime;
        [Header("Strafe")]
        [SerializeField] private float strafeRadius;
        [SerializeField] private float strafeSpeed;
        [Header("Wander")]
        [SerializeField] private float minWanderDistance;
        [SerializeField] private float maxWanderDistance;
        [SerializeField] private float wanderSpeed;
        [SerializeField] private float minWanderDuration;
        [SerializeField] private float maxWanderDuration;
        [Header("Attack")]
        [SerializeField] private float attackMovementSpeed;
        [Header("Returning area")]
        [SerializeField] private float distanceToArea;
        [SerializeField] private float teleportDuration;
        [SerializeField] private GameObject teleportGFX;

        private ARTGF_AIStateMachine _stateMachine;

        protected override void HandleSpawn()
        {
            base.HandleSpawn();
            SetupAI();
        }

        private void SetupAI()
        {
            _stateMachine = new ARTGF_AIStateMachine();

            _stateMachine.AddSensorTask(new ARTGF_AngerTypesSensor(this, c => c is ARTGF_Player, angerRadius, targetLostTime));

            _stateMachine.AddState(new ARTGF_RangedAttackNode(this, GetComponent<BombMobAttackHandler>(), attackMovementSpeed));
            _stateMachine.AddState(new ARTGF_StrafeState(this, strafeRadius, strafeSpeed, 1.5f));
            _stateMachine.AddState(new ARTGF_TeleportToAreaNode(this, distanceToArea, teleportDuration, teleportGFX));
            _stateMachine.AddState(new ARTGF_WanderAroundNode(this, minWanderDistance, maxWanderDistance, wanderSpeed, minWanderDuration, maxWanderDuration));
        }

        protected override void HandleLifeUpdate()
        {
            base.HandleLifeUpdate();
            _stateMachine.Tick();
        }
    }
}
