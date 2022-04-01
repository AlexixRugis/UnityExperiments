using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARTech.GameFramework.AI
{
    public class AngerTypesNode : Node
    {
        private readonly ILivingEntity _entity;
        private readonly Type[] _types;
        private readonly float _checkDistance;

        public AngerTypesNode(ILivingEntity entity, Type[] entityTypes, float checkDistance)
        {
            _entity = entity;
            _types = entityTypes;
            _checkDistance = checkDistance;
        }

        public override NodeState Evaluate()
        {
            if (_entity.Target != null) return NodeState.Failure;

            IEntity target = _entity.GetNearest(_checkDistance, _types);
            if (target != null)
            {
                _entity.Target = target as ILivingEntity;
                return NodeState.Success;
            }

            return NodeState.Failure;
        }
    }
}