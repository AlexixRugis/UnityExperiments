using UnityEngine;

namespace ARTech.GameFramework.AI
{
    public class RangedAttackNode : AIState
    {
        private readonly Character _host;
        private readonly IMovement _agent;
        private readonly IAttackHandler _handler;
        private readonly float _movementSpeed;

        private float _lastAttackTime;
        private bool _isPerforming;

        public RangedAttackNode(Character host, IAttackHandler handler,
            float movementSpeed)
        {
            _host = host;
            _agent = host.MovementController;
            _handler = handler;
            _movementSpeed = movementSpeed;

            _lastAttackTime = 0;
        }

        public override void OnEnterState()
        {
            base.OnEnterState();
            _isPerforming = false;
        }

        public override bool CanEnter() => _host.BattleTarget && Time.time - _lastAttackTime > _handler.Cooldown && !_handler.IsPerforming;

        public override bool CanExit() => false;

        public override AIStateResult Evaluate()
        {
            Character target = _host.BattleTarget;
            if (!target) return AIStateResult.Success;

            float distance = (_host.transform.position - target.transform.position).magnitude;
            bool canSeeTarget = _host.CanSee(target, float.MaxValue);

            if (canSeeTarget)
            {
                _agent.Speed = _movementSpeed;
                Vector3 targetPosition = Vector3.Lerp(_host.transform.position, target.transform.position, 1f - (_handler.AttackDistance / distance));
                _agent.TryMove(targetPosition);
            }

            if (!_isPerforming && canSeeTarget)
            {
                if (distance <= _handler.AttackDistance + 3f)
                {
                    _handler.Attack(target);
                    _isPerforming = true;
                }
            }
            else if (_isPerforming)
            {
                if (_handler.IsPerforming) _lastAttackTime = Time.time;
                else return AIStateResult.Success;
            }

            return AIStateResult.Running;
        }
    }
}