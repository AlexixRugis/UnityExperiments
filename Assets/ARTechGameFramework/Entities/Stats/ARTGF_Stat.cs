using UnityEngine;

namespace ARTech.GameFramework
{
    [System.Serializable]
    public struct ARTGF_Stat
    {
        [SerializeField] private float baseValue;

        private float _modifier;

        public float Value => Mathf.Clamp(baseValue + _modifier, 0f, baseValue + _modifier);

        public ARTGF_Stat(float baseValue)
        {
            this.baseValue = baseValue;
            _modifier = 0f;
        }

        public void AddModifier(float modifier)
        {
            _modifier += modifier;
        }

        public void RemoveModifier(float modifier)
        {
            _modifier -= modifier;
        }
    }
}