using System;
using UnityEngine;

namespace ARTech.GameFramework.AI
{
    public class ARTGF_AvoidTypesNode : ARTGF_AIState
    {
        private readonly ARTGF_Character _host;
        private readonly ARTGF_IMovement _agent;
        private readonly float _checkDistance;
        private readonly Predicate<ARTGF_Character> _match;
        private readonly float _runSpeed;

        private ARTGF_Character _avoidTarget;

        public ARTGF_AvoidTypesNode(ARTGF_Character host, float checkDistance, Predicate<ARTGF_Character> match, float runSpeed)
        {
            _host = host;
            _agent = host.MovementController;
            _checkDistance = checkDistance;
            _match = match;
            _runSpeed = runSpeed;
        }

        public override bool CanEnter()
        {
            return ARTGF_Utils.GetNearest(_host.transform.position, _checkDistance, _match);
        }

        public override bool CanExit() => true;

        public override ARTGF_AIStateResult Evaluate()
        {
            _avoidTarget = ARTGF_Utils.GetNearest(_host.transform.position, _checkDistance, _match); ;
            if (_avoidTarget == null) return ARTGF_AIStateResult.Success;

            _agent.Speed = _runSpeed;
            _agent.TryMove(
                ARTGF_Utils.GetPositionFrom(
                    _host.transform.position,
                    _avoidTarget.transform.position,
                    _host.MovementController.AgentRadius,
                    2f
                )
            );

            return ARTGF_AIStateResult.Running;
        }

        
    }
}