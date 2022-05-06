using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ARTech.GameFramework
{
    [RequireComponent(typeof(Character))]
    public class CharacterStage : MonoBehaviour
    {
        [System.Serializable]
        public struct Stage
        {
            [Range(0f, 1f)]
            [SerializeField] private float minHealthPercents;
            public UnityEvent OnStageEnter;
            public float MinHealthPercents => minHealthPercents;
        }

        [SerializeField] private Stage[] stages;
        private int _currentStage = -1;
        private Character _character;

        private void Awake()
        {
            _character = GetComponent<Character>();
            _character.OnHealthChanged.AddListener(HandleHealthChange);
        }

        private void OnDestroy()
        {
            _character.OnHealthChanged.RemoveListener(HandleHealthChange);
        }

        private void HandleHealthChange(Character character)
        {
            float healthPercents = character.CurrentHealth / character.MaxHealth.Value;

            for (int i = 0; i < stages.Length; i++)
            {
                if (healthPercents < stages[i].MinHealthPercents && _currentStage < i)
                {
                    _currentStage = i;
                    stages[i].OnStageEnter?.Invoke();
                }
            }
        }
    }
}