using System;
using UnityEngine;

namespace ARTech.GameFramework.AI
{
    public class AngerTypesSensor : AISensorTask
    {
        private readonly Character _host;
        private readonly Predicate<Character> _match;
        private readonly float _radius;
        private readonly float _targetLostTime;

        private float _lastTargetSeeTime;

        public AngerTypesSensor(Character host, Predicate<Character> match, float radius, float targetLostTime)
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
                if (_host.CanSee(_host.BattleTarget, _radius))
                {
                    _lastTargetSeeTime = Time.time;
                }

                if (Time.time - _lastTargetSeeTime > _targetLostTime)
                {
                    _host.BattleTarget = null;
                }

                return;
            }

            Character target = _host.GetNearest(_match, _radius);
            if (target != null)
            {
                _host.BattleTarget = target;
                return;
            }
        }
    }
}