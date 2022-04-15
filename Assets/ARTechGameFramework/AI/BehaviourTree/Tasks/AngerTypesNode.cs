using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARTech.GameFramework.AI
{
    public class AngerTypesNode : Node
    {
        private readonly INPC _host;
        private readonly Predicate<ICharacter> _match;

        public AngerTypesNode(INPC host, Predicate<ICharacter> match)
        {
            _host = host;
            _match = match;
        }

        public override NodeState Evaluate()
        {
            if (_host.BattleTarget != null) return NodeState.Failure;

            ICharacter target = _host.GetNearest(_match);
            if (target != null)
            {
                _host.BattleTarget = target;
                return NodeState.Success;
            }

            return NodeState.Failure;
        }
    }
}