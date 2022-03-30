using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ARTech.GameFramework.AI;
using ARTech.GameFramework;

namespace Mobs
{
    [RequireComponent(typeof(ARTech.GameFramework.IMovement))]
    public class JadeGoastWispBT : LivingEntity
    {
        [Header("Patrol")]
        [SerializeField] private float _patrolMovementDistance;
        [SerializeField] private float _patrolMovementSpeed;
        [SerializeField] private float _restTime;
        [Header("Attack")]
        [SerializeField] private float _attackCheckDistance;
        [SerializeField] private float _attackMovementSpeed;
        [SerializeField] private float _attackStopDistance;
        [SerializeField] private float _attackDodgeDistance;
        [SerializeField] private float _attackDodgeCooldown;
        [SerializeField] private float _attackCooldown;
        [Header("Teleport")]
        [SerializeField] private float _teleportCheckRadius;
        [SerializeField] private float _teleportMovementDistance;
        [SerializeField] private float _teleportCooldown;
        [Header("Avoid")]
        [SerializeField] private float _runAwayCheckRadius;
        [SerializeField] private float _runAwaySpeed;
        

        private ARTech.GameFramework.IMovement _agent;

        private BehaviorTree _tree = new BehaviorTree();

        protected void Awake()
        {
            _agent = GetComponent<ARTech.GameFramework.IMovement>();

            SetupTree();
        }

        protected override void Update()
        {
            base.Update();

            _tree.Tick();
        }

        private void SetupTree()
        {
            var attackHandler = GetComponent<IRangedAttackHandler>();
            Node root = new Selector(new List<Node>() {
                    new AngerTypesNode(this, new[] { typeof(Player) }, _attackCheckDistance),

                    new TeleportFromTypesNode(this, _agent, _teleportCheckRadius, new[] { typeof(Player) }, _teleportCooldown, _teleportMovementDistance),
                    new AvoidTypesNode(this, _agent, _runAwayCheckRadius, new[] { typeof(Player) }, _runAwaySpeed),
                    new RangedAttackNode(this, _agent, attackHandler, _attackMovementSpeed, _attackStopDistance, _attackDodgeDistance, _attackDodgeCooldown, _attackCooldown),
                    new WanderAroundNode(this, _agent, _patrolMovementDistance, _patrolMovementSpeed, _restTime)
                });

            _tree.SetNodes(root);
        }

        protected override void OnDrawGizmosSelected()
        {
            base.OnDrawGizmosSelected();

            if (!DebugEnabled) return;
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _attackCheckDistance);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _runAwayCheckRadius);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _teleportCheckRadius);
        }
    }
}