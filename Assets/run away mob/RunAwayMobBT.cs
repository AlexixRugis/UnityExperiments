using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ARTech.GameFramework;
using ARTech.GameFramework.AI;

namespace Mobs
{
    [RequireComponent(typeof(AIAgent))]
    public class RunAwayMobBT : AICharacter
    {
        [SerializeField] private float _runSpeed;
        [SerializeField] private float _patrolSpeed;
        [SerializeField] private float _patrolDistance;
        [SerializeField] private float _restTime;

        private BehaviorTree _tree = new BehaviorTree();

        protected override void HandleSpawn()
        {
            base.HandleSpawn();

            SetupTree();
        }

        protected override void HandleLifeUpdate()
        {
            base.HandleLifeUpdate();
            _tree.Tick();
        }

        protected void SetupTree()
        {
            Node root = new Selector(new List<Node>()
            {
                new AvoidTypesNode(this, 5f, e => e is Player, _runSpeed),
                new WanderAroundNode(this, _patrolDistance, _patrolSpeed, _restTime)
            });

            _tree.SetNodes(root);
        }
    }
}