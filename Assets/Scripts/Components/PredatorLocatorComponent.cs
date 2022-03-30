using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mobs
{
    public class PredatorLocatorComponent : MonoBehaviour
    {
        public float LocateRadius;

        private AttackableComponent[] _predators;

        private void Awake() => _predators = FindObjectsOfType<AttackableComponent>();

        public AttackableComponent GetNearestPredator()
        {
            AttackableComponent predator = null;
            float sqrMinDistance = LocateRadius*LocateRadius;

            foreach (var p in _predators)
            {
                if ((p.transform.position - transform.position).sqrMagnitude < sqrMinDistance)
                {
                    predator = p;
                    sqrMinDistance = (p.transform.position - transform.position).sqrMagnitude;
                }
            }

            return predator;
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, LocateRadius);
        }
#endif
    }
}