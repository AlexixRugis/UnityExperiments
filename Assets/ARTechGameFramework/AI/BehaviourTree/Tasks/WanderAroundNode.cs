using UnityEngine;

namespace ARTech.GameFramework.AI
{
    public class WanderAroundNode : Node
    {
        private readonly Character _host;
        private readonly IMovement _agent;
        private readonly float _patrolDistance;
        private readonly float _patrolSpeed;
        private readonly float _restDuration;

        private float _restStartTime;


        public WanderAroundNode(Character host, float patrolDistance, float speed, float restDuration)
        {
            _host = host;
            _agent = host.MovementController;
            _patrolDistance = patrolDistance;
            _patrolSpeed = speed;
            _restDuration = restDuration;

            _restStartTime = 0;
        }

        public override NodeState Evaluate()
        {
            if (_agent.HasPath())
            {
                if (_agent.GetRemainingDistance() > _patrolDistance * 2)
                {
                    _agent.ClearPath();
                }

                _restStartTime = Time.time;
                return NodeState.Running;
            }

            if (Time.time - _restStartTime > _restDuration)
            {
                if (_agent.TryMove(_host.GetRandomPositionAround(Random.Range(_patrolDistance * 0.5f, _patrolDistance)))) {
                    _agent.Speed = _patrolSpeed;
                    return NodeState.Running;
                } else if (_host.Area.GetDistance(_host.transform.position) > _patrolDistance)
                {
                    _agent.Teleport(_host.Area.GetRandomPointIn());
                }
            }

            return NodeState.Failure;
        }
    }
}