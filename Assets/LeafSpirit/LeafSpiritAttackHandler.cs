using ARTech.GameFramework;
using UnityEngine;

namespace Mobs
{
    [RequireComponent(typeof(LeafSpirit))]
    public class LeafSpiritAttackHandler : MonoBehaviour, IMeleeAttackHandler
    {
        private const string AnimatorTriggerName = "Attack";

        [SerializeField] private Animator animator;

        public void AttackMelee(Character damageable)
        {
            animator?.SetTrigger(AnimatorTriggerName);
        }
    }
}