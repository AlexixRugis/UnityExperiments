using UnityEngine;

namespace ARTech.GameFramework.AI
{
    public class WanderAroundNode : Node
    {
        private readonly ILivingEntity _entity;
        private readonly IMovement _agent;
        private readonly float _patrolDistance;
        private readonly float _patrolSpeed;
        private readonly float _restDuration;

        private float _restStartTime;


        public WanderAroundNode(ILivingEntity entity, IMovement agent, float patrolDistance, float speed, float restDuration)
        {
            _entity = entity;
            _agent = agent;
            _patrolDistance = patrolDistance;
            _patrolSpeed = speed;
            _restDuration = restDuration;

            _restStartTime = 0;
        }

        public override NodeState Evaluate()
        {
            if (_agent.HasPath())
            {
                _restStartTime = Time.time;
                return NodeState.Running;
            }

            if (Time.time - _restStartTime > _restDuration)
            {
                if (_agent.TryMove(_agent.GetRandomPositionAround(_entity.Position, Random.Range(_patrolDistance * 0.5f, _patrolDistance)))) {
                    _agent.Speed = _patrolSpeed;
                    return NodeState.Running;
                } else if (_agent.Area.GetDistance(_entity.Position) > _patrolDistance)
                {
                    _agent.Teleport(_agent.Area.GetRandomPointIn());
                }
            }

            return NodeState.Failure;
        }
    }
}