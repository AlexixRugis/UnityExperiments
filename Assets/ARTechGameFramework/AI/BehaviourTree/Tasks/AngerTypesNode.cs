using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARTech.GameFramework.AI
{
    public class AngerTypesNode : Node
    {
        private readonly ILivingEntity _entity;
        private readonly Predicate<IEntity> _match;
        private readonly float _checkDistance;

        public AngerTypesNode(ILivingEntity entity, Predicate<IEntity> match, float checkDistance)
        {
            _entity = entity;
            _match = match;
            _checkDistance = checkDistance;
        }

        public override NodeState Evaluate()
        {
            if (_entity.Target != null) return NodeState.Failure;

            IEntity target = _entity.GetNearest(_checkDistance, _match);
            if (target != null)
            {
                _entity.Target = target as IHealth;
                return NodeState.Success;
            }

            return NodeState.Failure;
        }
    }
}