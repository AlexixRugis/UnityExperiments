using ARTech.GameFramework;
using System.Collections;
using UnityEngine;

namespace Mobs
{
    [RequireComponent(typeof(LeafSpirit))]
    public sealed class LeafSpiritAttackHandler : AttackHandler
    {
        private const string AnimatorTriggerName = "Attack";

        [SerializeField] private float animationLength;
        [SerializeField] private Animator animator;

        protected override IEnumerator Perform(Character damageable)
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