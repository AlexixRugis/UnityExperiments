using UnityEngine;

namespace ARTech.GameFramework.AI
{
    public class WanderAroundNode : Node
    {
        private readonly LivingEntity _entity;
        private readonly IMovement _agent;
        private readonly float _patrolDistance;
        private readonly float _patrolSpeed;
        private readonly float _restDuration;

        private float _restStartTime;


        public WanderAroundNode(LivingEntity entity, IMovement agent, float patrolDistance, float speed, float restDuration)
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
                if (_agent.TryMove(_agent.GetRandomPositionAround(_entity.GetLocation(), Random.Range(_patrolDistance * 0.5f, _patrolDistance)))) {
                    _agent.Speed = _patrolSpeed;
                    return NodeState.Running;
                }
            }

            return NodeState.Failure;
        }
    }
}