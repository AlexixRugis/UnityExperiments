using UnityEngine;

namespace ARTech.GameFramework.AI
{
    public class LookAtTargetNode : Node
    {
        private LivingEntity _entity;

        public LookAtTargetNode(LivingEntity entity)
        {
            _entity = entity;
        }

        public override NodeState Evaluate()
        {
            _entity.transform.LookAt(_entity.GetTarget().transform);
            
            State = NodeState.Success;
            return State;
        }
    }
}