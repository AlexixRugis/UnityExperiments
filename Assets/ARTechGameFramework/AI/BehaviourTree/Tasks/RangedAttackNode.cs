using UnityEngine;

namespace ARTech.GameFramework.AI
{
    public class RangedAttackNode : Node
    {
        private readonly ILivingEntity _entity;
        private readonly IMovement _agent;
        private readonly IRangedAttackHandler _handler;
        private readonly float _movementSpeed;
        private readonly float _stoppingDistance;
        private readonly float _dodgeDistance;
        private readonly float _dodgeCooldown;
        private readonly float _cooldown;

        private float _lastAttackTime;
        private float _lastDodgeTime;

        public RangedAttackNode(ILivingEntity entity, IMovement agent, IRangedAttackHandler handler,
            float movementSpeed, float stoppingDistance,
            float dodgeDistance, float dodgeCooldown, float cooldown)
        {
            _entity = entity;
            _agent = agent;
            _handler = handler;
            _movementSpeed = movementSpeed;
            _stoppingDistance = stoppingDistance;
            _dodgeDistance = dodgeDistance;
            _dodgeCooldown = dodgeCooldown;
            _cooldown = cooldown;

            _lastAttackTime = 0;
        }
        public override NodeState Evaluate()
        {
            ILivingEntity target = _entity.Target;
            if (target == null) return NodeState.Failure;

            float distance = (_entity.Location - target.Location).magnitude;
            bool canSeeTarget = _entity.CanSee(target);

            if (distance <= _stoppingDistance + 3f)
            {
                if (canSeeTarget && Time.time - _lastAttackTime >= _cooldown)
                {
                    _agent.ClearPath();
                    _handler.AttackRanged(target);
                    _lastAttackTime = Time.time;
                } else if (Time.time - _lastDodgeTime > _dodgeCooldown)
                {
                    if (!_agent.HasPath())
                    {
                        _agent.TryMove(
                            _agent.GetRandomPositionAround(_entity.Location, _dodgeDistance)
                        );
                    } else
                    {
                        _lastDodgeTime = Time.time;
                    }
                }
            }
            else
            {
                _agent.Speed = _movementSpeed;

                Vector3 targetPosition = Vector3.Lerp(_entity.Location, target.Location, 1f - (_stoppingDistance / distance));

                _agent.TryMove(targetPosition);
            }

            return NodeState.Running;
        }
    }
}