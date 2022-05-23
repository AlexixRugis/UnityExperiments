using UnityEngine;

namespace ARTech.GameFramework.AI
{
    public class MeleeAttackNode : AIState
    {
        private readonly Character _host;
        private readonly IMovement _agent;
        private readonly IAttackHandler _handler;
        private readonly float _movementSpeed;

        private float _lastAttackTime;
        private bool _isPerforming;

        public MeleeAttackNode(Character host, IAttackHandler handler,
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

        public override bool CanEnter() => _host.BattleTarget &&
            Time.time - _lastAttackTime > _handler.Cooldown &&
            !_handler.IsPerforming;

        public override bool CanExit() => false;

        public override AIStateResult Evaluate()
        {
            Character target = _host.BattleTarget;
            if (!target) return AIStateResult.Success;

            float distance = (_host.transform.position - target.transform.position).magnitude;
            bool canSeeTarget = _host.CanSee(target, float.MaxValue);

            if (!_isPerforming)
            {
                _agent.Speed = _movementSpeed;
                Vector3 direction = target.transform.position - _host.transform.position;
                _agent.FocusPoint = target.transform.position;
                if (distance < _handler.MinAttackDistance)
                {
                    _agent.TryMove(_host.transform.position - direction);
                }
                else if (distance > _handler.MaxAttackDistance)
                {
                    _agent.TryMove(_host.transform.position + direction);
                }
                else
                {
                    _agent.FocusPoint = null;
                }
            }

            if (!_isPerforming && canSeeTarget)
            {
                if (distance <= _handler.MaxAttackDistance + 1f && distance >= _handler.MinAttackDistance - 1f)
                {
                    _agent.FocusPoint = null;
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