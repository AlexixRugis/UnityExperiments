using System.Collections.Generic;
using UnityEngine;
using ARTech.GameFramework.AI;
using ARTech.GameFramework;

namespace Mobs
{
    [RequireComponent(typeof(IMovement))]
    public class JadeGoastWispBT : Character
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

        private BehaviorTree _tree = new BehaviorTree();

        protected void Awake()
        {
            SetupTree();
        }

        protected override void HandleSpawn()
        {
            base.HandleSpawn();
            SetupTree();
        }

        protected override void HandleLifeUpdate()
        {
            _tree.Tick();
        } 

        private void SetupTree()
        {
            var attackHandler = GetComponent<IRangedAttackHandler>();
            Node root = new Selector(new List<Node>() {
                    new AngerTypesNode(this, e => e is Player, _attackCheckDistance),

                    new TeleportFromTypesNode(this, _teleportCheckRadius, e => e is Player, _teleportCooldown, _teleportMovementDistance),
                    new AvoidTypesNode(this, _runAwayCheckRadius, e => e is Player, _runAwaySpeed),
                    new RangedAttackNode(this, attackHandler, _attackMovementSpeed, _attackStopDistance, _attackDodgeDistance, _attackDodgeCooldown, _attackCooldown),
                    new WanderAroundNode(this, _patrolMovementDistance, _patrolMovementSpeed, _restTime)
                });

            _tree.SetNodes(root);
        }

        protected void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _attackCheckDistance);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _runAwayCheckRadius);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _teleportCheckRadius);
        }
    }
}