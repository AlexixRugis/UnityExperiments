using System.Collections.Generic;
using UnityEngine;
using ARTech.GameFramework.AI;
using ARTech.GameFramework;

namespace Mobs
{
    [RequireComponent(typeof(IMovement))]
    public sealed class JadeGoastWispBT : Character
    {
        [Header("Patrol")]
        [SerializeField] private float _minPatrolDistance;
        [SerializeField] private float _maxPatrolDistance;
        [SerializeField] private float _patrolMovementSpeed;
        [SerializeField] private float _minPatrolDuration;
        [SerializeField] private float _maxPatrolDuration;
        [Header("Attack")]
        [SerializeField] private float _attackCheckDistance;
        [SerializeField] private float _attackMovementSpeed;
        [SerializeField] private float _targetLostTime;
        [Header("Teleport")]
        [SerializeField] private float _teleportCheckRadius;
        [SerializeField] private float _teleportMovementDistance;
        [SerializeField] private float _teleportCooldown;
        [Header("Strafe")]
        [SerializeField] private float _strageRadius;
        [SerializeField] private float _strafeSpeed;
        [Header("Return Area")]
        [SerializeField] private float returnDistance;
        [SerializeField] private float returnDuration;
        [SerializeField] private GameObject returnGfx;

        private AIStateMachine _stateMachine = new AIStateMachine();

        protected override void HandleSpawn()
        {
            base.HandleSpawn();
            SetupAI();
        }

        protected override void HandleLifeUpdate()
        {
            _stateMachine.Tick();
        } 

        private void SetupAI()
        {
            var attackHandler = GetComponent<IAttackHandler>();

            _stateMachine.AddSensorTask(new AngerTypesSensor(this, e => e is Player, _attackCheckDistance, _targetLostTime));

            _stateMachine.AddState(new TeleportFromTypesNode(this, _teleportCheckRadius, e => e is Player, _teleportCooldown, _teleportMovementDistance));
            _stateMachine.AddState(new RangedAttackNode(this, attackHandler, _attackMovementSpeed));
            _stateMachine.AddState(new StrafeState(this, _strageRadius, _strafeSpeed, 2f));
            _stateMachine.AddState(new TeleportToAreaNode(this, returnDistance, returnDuration, returnGfx));
            _stateMachine.AddState(new WanderAroundNode(this, _minPatrolDistance, _maxPatrolDistance, _patrolMovementSpeed, _minPatrolDuration, _maxPatrolDuration));
        }

        protected void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _attackCheckDistance);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _teleportCheckRadius);
        }
    }
}