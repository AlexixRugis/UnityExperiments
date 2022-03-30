using System;
using UnityEngine;

namespace Mobs
{
    public abstract class StateComponent : MonoBehaviour
    {
        protected FSMComponent StateMachine { get; private set; }

        public virtual void Initialize(FSMComponent stateMachine)
        {
            if (!stateMachine) throw new ArgumentNullException(nameof(stateMachine));
            StateMachine = stateMachine;
        }

        public virtual void OnEnterState() { }

        public virtual void OnExitState() { }

        public virtual void OnUpdateState() { }
    }
}