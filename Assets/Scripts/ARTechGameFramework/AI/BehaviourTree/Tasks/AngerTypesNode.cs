using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARTech.GameFramework.AI
{
    public class AngerTypesNode : Node
    {
        private readonly LivingEntity _entity;
        private readonly Type[] _types;
        private readonly float _checkDistance;

        public AngerTypesNode(LivingEntity entity, Type[] entityTypes, float checkDistance)
        {
            _entity = entity;
            _types = entityTypes;
            _checkDistance = checkDistance;
        }

        public override NodeState Evaluate()
        {
            if (_entity.GetTarget()) return NodeState.Failure;

            Entity target = _entity.GetNearest(_checkDistance, _types);
            if (target)
            {
                _entity.SetTarget(target as LivingEntity);
                return NodeState.Success;
            }

            return NodeState.Failure;
        }
    }
}