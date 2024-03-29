using UnityEngine;

namespace ARTech.GameFramework.AI
{
    public class ARTGF_ChaseTargetState : ARTGF_AIState
    {
        private readonly ARTGF_Character _host;
        private readonly ARTGF_IMovement _agent;
        private readonly float _speed;
        private readonly float _targetDistance;

        public ARTGF_ChaseTargetState(ARTGF_Character host, float speed, float targetDistance)
        {
            _host = host;
            _agent = host.MovementController;
            _speed = speed;
            _targetDistance = targetDistance;
        }

        public override bool CanEnter() => _host.BattleTarget &&
            (_host.transform.position - _host.BattleTarget.transform.position)
            .sqrMagnitude > _targetDistance * _targetDistance;

        public override bool CanExit() => true;

        public override ARTGF_AIStateResult Evaluate()
        {
            ARTGF_Character target = _host.BattleTarget;
            if (target) return ARTGF_AIStateResult.Success;

            float distance = (_host.transform.position - target.transform.position).magnitude;
            _agent.Speed = _speed;
            _agent.TryMove(Vector3.Lerp(_host.transform.position, target.transform.position, 1f - (_targetDistance / distance)));

            return ARTGF_AIStateResult.Running;
        }
    }
}