using System.Collections.Generic;
using UnityEngine;
using ARTech.GameFramework.AI;
using ARTech.GameFramework;

namespace Mobs
{
    [RequireComponent(typeof(ARTGF_IMovement))]
    public sealed class JadeGoastWispBT : ARTGF_Character
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

        private ARTGF_AIStateMachine _stateMachine = new ARTGF_AIStateMachine();

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
            var attackHandler = GetComponent<ARTGF_IAttackHandler>();

            _stateMachine.AddSensorTask(new ARTGF_AngerTypesSensor(this, e => e is ARTGF_Player, _attackCheckDistance, _targetLostTime));

            _stateMachine.AddState(new ARTGF_TeleportFromTypesNode(this, _teleportCheckRadius, e => e is ARTGF_Player, _teleportCooldown, _teleportMovementDistance));
            _stateMachine.AddState(new ARTGF_RangedAttackNode(this, attackHandler, _attackMovementSpeed));
            _stateMachine.AddState(new ARTGF_StrafeState(this, _strageRadius, _strafeSpeed, 2f));
            _stateMachine.AddState(new ARTGF_TeleportToAreaNode(this, returnDistance, returnDuration, returnGfx));
            _stateMachine.AddState(new ARTGF_WanderAroundNode(this, _minPatrolDistance, _maxPatrolDistance, _patrolMovementSpeed, _minPatrolDuration, _maxPatrolDuration));
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