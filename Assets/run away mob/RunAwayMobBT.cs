using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ARTech.GameFramework;
using ARTech.GameFramework.AI;

namespace Mobs
{
    [RequireComponent(typeof(AIAgent))]
    public class RunAwayMobBT : NPC
    {
        [SerializeField] private float _runSpeed;
        [SerializeField] private float _patrolSpeed;
        [SerializeField] private float _patrolDistance;
        [SerializeField] private float _restTime;

        private AIAgent _agent;

        private BehaviorTree _tree = new BehaviorTree();

        protected void Awake()
        {
            _agent = GetComponent<AIAgent>();

            SetupTree();
        }

        private void SetupTree()
        {
            Node root = new Selector(new List<Node>()
            {
                new AvoidTypesNode(this, _agent, 5f, e => e is IPlayer, _runSpeed),
                new WanderAroundNode(this, _agent, _patrolDistance, _patrolSpeed, _restTime)
            });

            _tree.SetNodes(root);
        }

        protected override void OnLifeUpdate()
        {
            _tree.Tick();
        }
    }
}