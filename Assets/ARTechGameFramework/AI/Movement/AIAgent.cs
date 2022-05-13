using UnityEngine;
using UnityEngine.AI;

namespace ARTech.GameFramework.AI
{
    [RequireComponent(typeof(NavMeshAgent), typeof(Character))]
    public class AIAgent : MonoBehaviour, IMovement
    {
        private NavMeshAgent _agent;

        public bool HasReachedDestination => _agent.remainingDistance <= _agent.stoppingDistance;
        public float Velocity => _agent.velocity.magnitude;

        public float Speed { get => _agent.speed; set => _agent.speed = value; }

        public Vector3 CurrentVelocity => _agent.velocity;

        public Character Character { get; private set; }

        public float AgentRadius => _agent.radius;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            Character = GetComponent<Character>();
        }

        public void ClearPath()
        {
            _agent.ResetPath();
        }

        public bool HasPath()
        {
            return _agent.hasPath && _agent.velocity.magnitude > 0.1f;
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
    }
}