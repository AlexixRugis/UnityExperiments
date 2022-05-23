using ARTech.GameFramework;
using ARTech.GameFramework.AI;
using UnityEngine;

namespace Mobs
{
    [RequireComponent(typeof(BombMobAttackHandler))]
    public sealed class BombMob : Character
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

            _stateMachine.AddState(new RangedAttackNode(this, GetComponent<BombMobAttackHandler>(), attackMovementSpeed));
            _stateMachine.AddState(new StrafeState(this, strafeRadius, strafeSpeed, 1.5f));
            _stateMachine.AddState(new WanderAroundNode(this, minWanderDistance, maxWanderDistance, wanderSpeed, minWanderDuration, maxWanderDuration));
        }

        protected override void HandleLifeUpdate()
        {
            base.HandleLifeUpdate();
            _stateMachine.Tick();
        }
    }
}
