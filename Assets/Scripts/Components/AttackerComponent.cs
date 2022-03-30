using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mobs
{
    [RequireComponent(typeof(Collider), typeof(Rigidbody))]
    public class AttackerComponent : MonoBehaviour
    {
        [SerializeField] private List<AttackableType> _attackableTypes;

        private Collider _collider;

        private readonly List<AttackableComponent> _attackables = new List<AttackableComponent>();
        public IEnumerable<AttackableComponent> Attackables => _attackables;

        private void Awake()
        {
            _collider = GetComponent<Collider>();
            _collider.isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            AttackableComponent attackable = other.GetComponent<AttackableComponent>();
            if (attackable && _attackableTypes.Contains(attackable.Type))
            {
                _attackables.Add(attackable);
                attackable.Attackers.Add(this);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            AttackableComponent attackable = other.GetComponent<AttackableComponent>();
            if (attackable && _attackableTypes.Contains(attackable.Type))
            {
                _attackables.Remove(attackable);
                attackable.Attackers.Remove(this);
            }
        }

        public AttackableComponent GetNearestAttackable()
        {
            float minDistanceSqr = float.PositiveInfinity;
            AttackableComponent nearest = null;

            for (int i = 0; i < _attackables.Count; i++)
            {
                float sqrDistance = (_attackables[i].transform.position - transform.position).sqrMagnitude;
                if (sqrDistance < minDistanceSqr)
                {
                    minDistanceSqr = sqrDistance;
                    nearest = _attackables[i];
                }
            }

            return nearest;
        }
    }
}