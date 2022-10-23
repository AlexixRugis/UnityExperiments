using UnityEngine;

namespace ARTech.GameFramework
{
    [System.Serializable]
    public struct ARTGF_EntityStats
    {
        [SerializeField] private string name;
        [SerializeField] private ARTGF_Stat maxHealth;
        [SerializeField] private ARTGF_Stat protection;
        [SerializeField] private ARTGF_Stat damage;
        [SerializeField] private ARTGF_Stat movementSpeed;

        public string Name => name;
        public ARTGF_Stat MaxHealth => maxHealth;
        public ARTGF_Stat Protection => protection;
        public ARTGF_Stat Damage => damage;
        public ARTGF_Stat MovementSpeed => movementSpeed;
    }
}