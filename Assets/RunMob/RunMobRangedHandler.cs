using ARTech.GameFramework;
using System.Collections;
using UnityEngine;

namespace Mobs
{
    [RequireComponent(typeof(RunMob), typeof(IMovement))]
    public sealed class RunMobRangedHandler : AttackHandler
    {
        private const string AnimatorAttackParam = "RunAttack";

        [SerializeField] private float speed;
        [SerializeField] private float time;
        [SerializeField] private Animator animator;

        private float _startTime;
        private IMovement _agent;

        private void Awake()
        {
            _agent = GetComponent<IMovement>();
        }

        protected override IEnumerator Perform(Character target)
        {
            IsPerforming = true;

            Vector3 direction = Vector3.zero;
            float timer = Time.time;
            _agent.ClearPath();
            animator.SetBool(AnimatorAttackParam, true);

            while (Time.time - timer < 2f)
            {
                direction = target.transform.position - transform.position;
                direction.y = 0;
                direction.Normalize();
                transform.rotation = Quaternion.LookRotation(direction);

                yield return null;
            }

            _startTime = Time.time;
            while (Time.time - _startTime < time)
            {
                _agent.Speed = speed;
                _agent.TryMove(transform.position + direction);
                yield return null;
            }

            animator.SetBool(AnimatorAttackParam, false);
            IsPerforming = false;
        }
    }
}