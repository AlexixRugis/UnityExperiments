using System.Collections.Generic;
using UnityEngine;

namespace ARTech.GameFramework.AI
{
    public sealed class AIStateMachine
    {
        private readonly List<AIState> _states = new List<AIState>();
        private readonly List<AISensorTask> _sensors = new List<AISensorTask>();

        private AIState _currentState;

        public void AddState(AIState state)
        {
            _states.Add(state);
        }

        public void AddSensorTask(AISensorTask state)
        {
            _sensors.Add(state);
        }

        public void Tick()
        {
            EvaluateSensors();
            EvaluateState();

            Debug.Log(_currentState.GetType().Name);
        }

        private void EvaluateSensors()
        {
            for (int i = 0; i < _sensors.Count; i++)
            {
                _sensors[i].Evaluate();
            }
        }

        private void EvaluateState()
        {
            if (_currentState != null)
            {
                AIStateResult result = _currentState.Evaluate();

                if (result == AIStateResult.Success)
                {
                    _currentState.OnExitState();
                    _currentState = null;
                }
            }

            if (_currentState == null || _currentState.CanExit())
            {
                TryChangeState();
            }
        }

        private void TryChangeState()
        {
            for (int i = 0; i < _states.Count; i++)
            {
                AIState state = _states[i];

                if (state == _currentState)
                {
                    break;
                }

                if (state.CanEnter())
                {
                    if (_currentState != null) _currentState.OnExitState();
                    _currentState = state;
                    state.OnEnterState();
                    break;
                }
            }
        }
    }
}