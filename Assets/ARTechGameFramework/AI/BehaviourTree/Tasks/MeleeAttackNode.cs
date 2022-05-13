using UnityEngine;

namespace ARTech.GameFramework.AI
{
    public class MeleeAttackNode : Node
    {
        private Character _character;
        private IMovement _agent;
        private IMeleeAttackHandler _attackHandler;
        private float _movementSpeed;
        private float _attackDistance;
        private float _dodgeDistance;
        private float _dodgeCooldown;
        private float _cooldown;

        private float _lastAttackTime;
        private float _lastDodgeTime;

        public MeleeAttackNode(Character host, IMeleeAttackHandler handler,
            float movementSpeed, float attackDistance,
            float dodgeDistance, float dodgeCooldown, float cooldown)
        {
            _character = host;
            _agent = host.MovementController;
            _attackHandler = handler;
            _movementSpeed = movementSpeed;
            _attackDistance = attackDistance;
            _dodgeDistance = dodgeDistance;
            _dodgeCooldown = dodgeCooldown;
            _cooldown = cooldown;

            _lastAttackTime = 0;
            _lastDodgeTime = 0;
        }

        public override NodeState Evaluate()
        {
            Character target = _character.BattleTarget;
            if (target == null) return NodeState.Failure;

            float distance = (_character.transform.position - target.transform.position).magnitude;

            if (distance <= _attackDistance)
            {
                if (Time.time - _lastAttackTime >= _cooldown)
                {
                    _agent.ClearPath();
                    Vector3 direction = _character.transform.position - target.transform.position;
                    direction.y = 0;

                    _character.transform.rotation = Quaternion.LookRotation(-direction);
                    _attackHandler.AttackMelee(target);
                    _lastAttackTime = Time.time;
                }
                else if (Time.time - _lastDodgeTime > _dodgeCooldown)
                {
                    if (!_agent.HasPath())
                    {
                        _agent.TryMove(
                            _character.GetRandomPositionAround(_dodgeDistance)
                        );
                    }
                    else
                    {
                        _lastDodgeTime = Time.time;
                    }
                }
            }
            else
            {
                _agent.Speed = _movementSpeed;
                _agent.TryMove(target.transform.position);
            }

            return NodeState.Running;
        }
    }
}