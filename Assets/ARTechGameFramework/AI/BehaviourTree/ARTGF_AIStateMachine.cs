using System.Collections.Generic;
using UnityEngine;

namespace ARTech.GameFramework.AI
{
    public sealed class ARTGF_AIStateMachine
    {
        private readonly List<ARTGF_AIState> _states = new List<ARTGF_AIState>();
        private readonly List<ARTGF_AISensorTask> _sensors = new List<ARTGF_AISensorTask>();

        private ARTGF_AIState _currentState;

        public void AddState(ARTGF_AIState state)
        {
            _states.Add(state);
        }

        public void AddSensorTask(ARTGF_AISensorTask state)
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
                ARTGF_AIStateResult result = _currentState.Evaluate();

                if (result == ARTGF_AIStateResult.Success)
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
                ARTGF_AIState state = _states[i];

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