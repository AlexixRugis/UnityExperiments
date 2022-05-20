using ARTech.GameFramework;
using System.Collections;
using UnityEngine;

namespace Mobs
{
    [RequireComponent(typeof(RunMob))]
    public class RunMobMeleeHandler : MonoBehaviour, IAttackHandler
    {
        private const string AnimatorTriggerName = "Attack";

        [SerializeField] private float attackDistance;
        [SerializeField] private float cooldown;
        [SerializeField] private float animationLength;
        [SerializeField] private Animator animator;

        public float Cooldown => cooldown;
        public bool IsPerforming { get; private set; }
        public bool CanPerform => !IsPerforming;

        public float AttackDistance => attackDistance;

        public void Attack(Character damageable)
        {
            if (CanPerform)
                StartCoroutine(AttackRoutine(damageable));
        }

        private IEnumerator AttackRoutine(Character damageable)
        {
            IsPerforming = true;

            Vector3 direction = damageable.transform.position - transform.position;
            direction.y = 0;
            transform.rotation = Quaternion.LookRotation(direction);

            animator?.SetTrigger(AnimatorTriggerName);
            yield return new WaitForSeconds(animationLength);

            IsPerforming = false;
        }
    }
}