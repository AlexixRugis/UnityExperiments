using UnityEngine;
using ARTech.GameFramework;
using ARTech.GameFramework.AI;

namespace Mobs
{
    [RequireComponent(typeof(ARTGF_AIAgent))]
    public sealed class RunAwayMobBT : ARTGF_Character
    {
        [SerializeField] private float _runSpeed;
        [SerializeField] private float _patrolSpeed;
        [SerializeField] private float _minPatrolDistance;
        [SerializeField] private float _maxPatrolDistance;
        [SerializeField] private float _minPatrolDuration;
        [SerializeField] private float _maxPatrolDuration;

        private ARTGF_AIStateMachine _stateMachine = new ARTGF_AIStateMachine();

        protected override void HandleSpawn()
        {
            base.HandleSpawn();

            SetupAI();
        }

        protected override void HandleLifeUpdate()
        {
            base.HandleLifeUpdate();
            _stateMachine.Tick();
        }

        protected void SetupAI()
        {
            _stateMachine.AddState(new ARTGF_AvoidTypesNode(this, 5f, e => e is ARTGF_Player, _runSpeed));
            _stateMachine.AddState(new ARTGF_WanderAroundNode(this, _minPatrolDistance, _maxPatrolDistance, _patrolSpeed, _minPatrolDuration, _maxPatrolDuration));
        }
    }
}