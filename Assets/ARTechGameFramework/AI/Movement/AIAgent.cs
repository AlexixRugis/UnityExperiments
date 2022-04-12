using UnityEngine;
using UnityEngine.AI;

namespace ARTech.GameFramework.AI
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class AIAgent : MonoBehaviour, IMovement
    {
        [SerializeField] private Area _area;

        private NavMeshAgent _agent;

        private NavMeshPath _path;

        public bool HasReachedDestination => _agent.remainingDistance <= _agent.stoppingDistance;
        public float Velocity => _agent.velocity.magnitude;

        public float Speed { get => _agent.speed; set => _agent.speed = value; }

        public Vector3 CurrentVelocity => _agent.velocity;

        public IArea Area { get => _area; set => _area = value as Area; }

        private void Awake()
        {
            _path = new NavMeshPath();
            _agent = GetComponent<NavMeshAgent>();
        }

        public void ClearPath()
        {
            _agent.ResetPath();
        }

        public bool HasPath()
        {
            return _agent.hasPath;
        }

        public void SetStopped(bool isStopped)
        {
            _agent.isStopped = false;
        }

        public bool TryMove(Vector3? position)
        {
            if (position == null)
            {
                ClearPath();
                return false;
            }

            return _agent.SetDestination(position.Value);
        }

        public bool Teleport(Vector3? position)
        {
            if (position == null) return false;

            return _agent.Warp(position.Value);
        }

        public float GetRemainingDistance()
        {
            return _agent.remainingDistance;
        }

        public Vector3? GetRandomPositionAround(Vector3 center, float radius)
        {
            Vector3? position = _area.GetRandomPointAround(center, radius, _agent.radius);


            if (position != null) {
                NavMeshHit hit;
                NavMesh.SamplePosition(position.Value, out hit, radius, NavMesh.AllAreas);
                position = hit.position;

                _agent.CalculatePath(position.Value, _path);
                return NavMeshPathStatus.PathComplete == _path.status ? position : null;
            }

            return null;
        }

        public Vector3? GetPositionFrom(Vector3 center, Vector3 from, float radius)
        {
            Vector3 direction = (center - from);
            direction.y = 0;

            NavMeshHit hit;
            NavMesh.SamplePosition(center + direction.normalized * radius, out hit, radius, NavMesh.AllAreas);

            return hit.position;
        }
    }
}