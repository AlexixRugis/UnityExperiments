using UnityEngine;
using ARTech.GameFramework;
using ARTech.GameFramework.AI;

namespace Mobs
{
    [RequireComponent(typeof(AIAgent))]
    public sealed class RunAwayMobBT : Character
    {
        [SerializeField] private float _runSpeed;
        [SerializeField] private float _patrolSpeed;
        [SerializeField] private float _minPatrolDistance;
        [SerializeField] private float _maxPatrolDistance;
        [SerializeField] private float _minPatrolDuration;
        [SerializeField] private float _maxPatrolDuration;

        private AIStateMachine _stateMachine = new AIStateMachine();

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
            _stateMachine.AddState(new AvoidTypesNode(this, 5f, e => e is Player, _runSpeed));
            _stateMachine.AddState(new WanderAroundNode(this, _minPatrolDistance, _maxPatrolDistance, _patrolSpeed, _minPatrolDuration, _maxPatrolDuration));
        }
    }
}