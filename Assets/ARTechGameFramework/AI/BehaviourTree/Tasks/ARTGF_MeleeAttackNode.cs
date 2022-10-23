using UnityEngine;

namespace ARTech.GameFramework.AI
{
    public class ARTGF_MeleeAttackNode : ARTGF_AIState
    {
        private readonly ARTGF_Character _host;
        private readonly ARTGF_IMovement _agent;
        private readonly ARTGF_IAttackHandler _handler;
        private readonly float _movementSpeed;

        private float _lastAttackTime;
        private bool _isPerforming;

        public ARTGF_MeleeAttackNode(ARTGF_Character host, ARTGF_IAttackHandler handler,
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

        public override ARTGF_AIStateResult Evaluate()
        {
            ARTGF_Character target = _host.BattleTarget;
            if (!target) return ARTGF_AIStateResult.Success;

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
                    _agent.ClearPath();
                    _agent.FocusPoint = null;
                    _handler.Attack(target);
                    _isPerforming = true;
                }
            }
            else if (_isPerforming)
            {
                if (_handler.IsPerforming) _lastAttackTime = Time.time;
                else return ARTGF_AIStateResult.Success;
            }

            return ARTGF_AIStateResult.Running;
        }
    }
}