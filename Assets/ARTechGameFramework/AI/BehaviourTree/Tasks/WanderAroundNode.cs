using UnityEngine;

namespace ARTech.GameFramework.AI
{
    public class WanderAroundNode : Node
    {
        private readonly Character _host;
        private readonly IMovement _agent;
        private readonly float _minPatrolDistance;
        private readonly float _maxPatrolDistance;
        private readonly float _patrolSpeed;
        private readonly float _minPatrolDuration;
        private readonly float _maxPatrolDuration;

        private float _patrolStartTime;
        private float _patrolDuration;


        public WanderAroundNode(Character host, float minPatrolDistance, float maxPatrolDistance, float speed, float minPatrolDuration, float maxPatrolDuration)
        {
            _host = host;
            _agent = host.MovementController;
            _minPatrolDuration = minPatrolDuration;
            _maxPatrolDuration = maxPatrolDuration;
            _patrolSpeed = speed;
            _minPatrolDistance = minPatrolDistance;
            _maxPatrolDistance = maxPatrolDistance;

            _patrolStartTime = 0;
            _patrolDuration = Random.Range(_minPatrolDuration, _maxPatrolDuration);
        }

        public override NodeState Evaluate()
        {
            if (Time.time - _patrolStartTime > _patrolDuration)
            {
                float patrolDistance = Random.Range(_minPatrolDistance, _maxPatrolDistance);
                if (_agent.TryMove(_host.GetRandomPositionAround(patrolDistance))) {
                    _patrolStartTime = Time.time;
                    _patrolDuration = Random.Range(_minPatrolDuration, _maxPatrolDuration);
                    _agent.Speed = _patrolSpeed;
                    return NodeState.Running;
                } else if (_host.Area.GetDistance(_host.transform.position) > patrolDistance)
                {
                    _agent.Teleport(_host.Area.GetRandomPointIn());
                }
            }

            return NodeState.Failure;
        }
    }
}