using System.Collections;
using UnityEngine;


namespace ARTech.GameFramework
{
    public abstract class AttackHandler : MonoBehaviour, IAttackHandler
    {
        [SerializeField] private float cooldown;
        [SerializeField] private float minDistance;
        [SerializeField] private float maxDistance;

        public float Cooldown => cooldown;

        public float MinAttackDistance => minDistance;

        public float MaxAttackDistance => maxDistance;

        public bool IsPerforming { get; protected set; }

        public void Attack(Character damageable)
        {
            if (!IsPerforming)
                StartCoroutine(Perform(damageable));
        }

        protected abstract IEnumerator Perform(Character target);
    }
}