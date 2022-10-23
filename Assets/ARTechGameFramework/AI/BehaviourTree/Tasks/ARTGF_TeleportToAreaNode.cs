using UnityEngine;

namespace ARTech.GameFramework.AI
{
    public class ARTGF_TeleportToAreaNode : ARTGF_AIState
    {
        private readonly ARTGF_Character _host;
        private readonly ARTGF_IMovement _movement;
        private readonly float _distanceToArea;
        private readonly float _teleportDuration;
        private readonly GameObject _teleportGFX;

        private float _startTeleportTime;
        private bool _hasTeleported;

        public ARTGF_TeleportToAreaNode(ARTGF_Character character, float distanceToArea, float teleportDuration, GameObject teleportGFX)
        {
            _host = character;
            _movement = character.MovementController;
            _teleportGFX = teleportGFX;

            _distanceToArea = distanceToArea;
            _teleportDuration = teleportDuration;

            _startTeleportTime = 0f;
        }

        public override void OnEnterState()
        {
            base.OnEnterState();
            _movement.ClearPath();
            _host.IsImmortal = true;
            _teleportGFX.SetActive(true);
            _startTeleportTime = Time.time;
            _hasTeleported = false;
        }

        public override void OnExitState()
        {
            base.OnExitState();
            _host.IsImmortal = false;
            _teleportGFX.SetActive(false);
        }

        public override ARTGF_AIStateResult Evaluate()
        {
            if (!_hasTeleported && Time.time - _startTeleportTime > _teleportDuration * 0.5f)
            {
                if (_movement.Teleport(_host.Area.GetRandomPointIn()))
                {
                    _hasTeleported = true;
                }
            }
            else if (Time.time - _startTeleportTime > _teleportDuration)
            {
                return ARTGF_AIStateResult.Success;
            }

            return ARTGF_AIStateResult.Running;
        }

        public override bool CanExit() => false;
        public override bool CanEnter() =>
            (_host.Area.GetClosestPoint(_host.transform.position) - _host.transform.position)
            .sqrMagnitude > _distanceToArea * _distanceToArea;
    }
}