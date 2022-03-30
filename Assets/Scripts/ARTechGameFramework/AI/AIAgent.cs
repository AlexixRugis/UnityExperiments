using UnityEngine;
using UnityEngine.AI;

namespace ARTech.GameFramework.AI
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class AIAgent : MonoBehaviour, IMovement
    {
        private NavMeshAgent _agent;

        private NavMeshPath _path;

        public bool HasReachedDestination => _agent.remainingDistance <= _agent.stoppingDistance;
        public float Velocity => _agent.velocity.magnitude;

        public float Speed { get => _agent.speed; set => _agent.speed = value; }

        public Vector3 CurrentVelocity => _agent.velocity;

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

        public Vector3? GetRandomPoint(Vector3 center, float radius)
        {
            bool hasCorrectPoint = false;
            Vector3 position = Vector3.positiveInfinity;
            while (!hasCorrectPoint)
            {
                NavMeshHit hit;
                NavMesh.SamplePosition(Random.onUnitSphere * radius + center, out hit, radius, NavMesh.AllAreas);
                position = hit.position;

                _agent.CalculatePath(position, _path);
                hasCorrectPoint = NavMeshPathStatus.PathComplete == _path.status;
            }

            return position;
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
            bool hasCorrectPoint = false;
            Vector3 position = Vector3.positiveInfinity;
            while (!hasCorrectPoint)
            {
                NavMeshHit hit;
                NavMesh.SamplePosition(Random.onUnitSphere * radius + center, out hit, radius, NavMesh.AllAreas);
                position = hit.position;

                _agent.CalculatePath(position, _path);
                hasCorrectPoint = NavMeshPathStatus.PathComplete == _path.status;
            }

            return position;
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