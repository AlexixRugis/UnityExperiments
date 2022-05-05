using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARTech.GameFramework.AI
{
    public class AngerTypesNode : Node
    {
        private readonly AICharacter _host;
        private readonly Predicate<AICharacter> _match;
        private readonly float _radius;

        public AngerTypesNode(AICharacter host, Predicate<AICharacter> match, float radius)
        {
            _host = host;
            _match = match;
            _radius = radius;
        }

        public override NodeState Evaluate()
        {
            if (_host.BattleTarget != null) return NodeState.Failure;

            AICharacter target = _host.GetNearest(_match, _radius);
            if (target != null)
            {
                _host.BattleTarget = target;
                return NodeState.Success;
            }

            return NodeState.Failure;
        }
    }
}