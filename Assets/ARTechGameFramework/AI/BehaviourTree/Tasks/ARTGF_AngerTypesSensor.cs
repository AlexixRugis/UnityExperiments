using System;
using UnityEngine;

namespace ARTech.GameFramework.AI
{
    public class ARTGF_AngerTypesSensor : ARTGF_AISensorTask
    {
        private readonly ARTGF_Character _host;
        private readonly Predicate<ARTGF_Character> _match;
        private readonly float _radius;
        private readonly float _targetLostTime;

        private float _lastTargetSeeTime;

        public ARTGF_AngerTypesSensor(ARTGF_Character host, Predicate<ARTGF_Character> match, float radius, float targetLostTime)
        {
            _host = host;
            _match = match;
            _radius = radius;
            _targetLostTime = targetLostTime;

            _lastTargetSeeTime = 0;
        }

        public override void Evaluate()
        {
            if (_host.BattleTarget)
            {
                if (ARTGF_Utils.CanSee(_host.transform.position, _host.BattleTarget, _radius))
                {
                    _lastTargetSeeTime = Time.time;
                }

                if (Time.time - _lastTargetSeeTime > _targetLostTime)
                {
                    _host.BattleTarget = null;
                }

                return;
            }

            ARTGF_Character target = ARTGF_Utils.GetNearest(_host.transform.position, _radius, _match);
            if (target != null)
            {
                _host.BattleTarget = target;
                return;
            }
        }
    }
}