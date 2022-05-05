using UnityEngine;

namespace ARTech.GameFramework
{
    public abstract class NPC : Character, INPC
    {
        [Header("Movement")]
        [SerializeField] private float _agentRadius;
        [SerializeField] private LayerMask _obstaclesMask;

        [Header("Targeting")]
        [SerializeField] private float _loseTargetDuration;

        public IArea Area { get; set; }
        public ICharacter BattleTarget { get; set; }
        public float LastTargetSeeTime { get; private set; }

        private void Update()
        {
            if (!IsRemoved)
            {
                if (BattleTarget != null)
                {
                    if (Area.GetDistance(Position) < 1f && CanSee(BattleTarget))
                    {
                        LastTargetSeeTime = Time.time;
                    }

                    if (Time.time - LastTargetSeeTime > _loseTargetDuration)
                    {
                        BattleTarget = null;
                    }
                }

                OnLifeUpdate();
            }

        }

        protected abstract void OnLifeUpdate();

        public Vector3? GetPositionFrom(Vector3 from, float radius)
        {
            Vector3 directionNormalized = (Position - from).normalized;

            if (!Physics.SphereCast(transform.position, _agentRadius, directionNormalized, out RaycastHit hit, radius, _obstaclesMask))
            {
                return Position + directionNormalized * radius;
            }
            else
            {
                float distance = hit.distance - _agentRadius;
                if (distance <= 0)
                {
                    return Position;
                }

                return Position + directionNormalized * (hit.distance - _agentRadius);
            }
        }

        public Vector3? GetRandomPositionAround(float radius)
        {
            return Area?.GetRandomPointAround(Position, radius, _agentRadius);
        }
    }
}