using UnityEngine;

namespace Mobs
{
    [System.Serializable]
    public struct AIEntityStats
    {
        [SerializeField] private float _lookRadius;
        [SerializeField] private float _patrolDistance;
        
        public float LookRadius => _lookRadius;
        public float PatrolDistance => _patrolDistance;
    }
}