using UnityEngine;

namespace ARTech.GameFramework
{
    [System.Serializable]
    public struct EntityStats
    {
        [SerializeField] private string name;
        [SerializeField] private Stat maxHealth;
        [SerializeField] private Stat protection;
        [SerializeField] private Stat damage;
        [SerializeField] private Stat movementSpeed;

        public string Name => name;
        public Stat MaxHealth => maxHealth;
        public Stat Protection => protection;
        public Stat Damage => damage;
        public Stat MovementSpeed => movementSpeed;
    }
}