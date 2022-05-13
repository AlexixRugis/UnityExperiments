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
        [SerializeField] private float attackRadius;
        [SerializeField] private float angerRadius;
        [SerializeField] private float attackMovementSpeed;
        [SerializeField] private float attackCooldown;
        [SerializeField] private float dodgeDistance;
        [SerializeField] private float dodgeCooldown;

        [Header("Wander")]
        [SerializeField] private float minWanderDistance;
        [SerializeField] private float maxWanderDistance;
        [SerializeField] private float wanderSpeed;
        [SerializeField] private float minPatrolDuration;
        [SerializeField] private float maxPatrolDuration;

        private BehaviorTree _tree = new BehaviorTree();
        private LeafSpiritAttackHandler _attackHandler;

        protected override void HandleSpawn()
        {
            base.HandleSpawn();
            _attackHandler = GetComponent<LeafSpiritAttackHandler>();
            SetupTree();
        }

        private void SetupTree()
        {
            Node root = new Selector(new List<Node>{
                new AngerTypesNode(this, c => c is Player, angerRadius),

                new MeleeAttackNode(this, _attackHandler, attackMovementSpeed, attackRadius, dodgeDistance, dodgeCooldown, attackCooldown),
                new WanderAroundNode(this, minWanderDistance, maxWanderDistance, wanderSpeed, minPatrolDuration, maxPatrolDuration),
            });

            _tree.SetNodes(root);
        }

        protected override void HandleLifeUpdate()
        {
            base.HandleLifeUpdate();
            _tree.Tick();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, angerRadius);
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, attackRadius);
        }
    }
}