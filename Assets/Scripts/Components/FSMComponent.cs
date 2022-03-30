using System.Collections.Generic;
using UnityEngine;
using System;

namespace Mobs
{
    public abstract class FSMComponent : MonoBehaviour
    {
        private readonly List<StateComponent> _states = new List<StateComponent>();
        public IReadOnlyList<StateComponent> States => _states;
        public StateComponent CurrentState { get; private set; }

        protected void TransitToState(StateComponent state)
        {
            if (!_states.Contains(state)) throw new InvalidOperationException(nameof(state));

            if (CurrentState) CurrentState.OnExitState();

            CurrentState = state;

            if (CurrentState) CurrentState.OnEnterState();
        }

        protected virtual void Update()
        {
            if (CurrentState) CurrentState.OnUpdateState();
        }

        protected void RegisterState(StateComponent component)
        {
            if (!component) throw new ArgumentNullException(nameof(component));

            component.Initialize(this);
            _states.Add(component);
        }
    }
}