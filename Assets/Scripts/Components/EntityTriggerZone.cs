using System;
using UnityEngine;

namespace Mobs
{
    public class EntityTriggerZone : MonoBehaviour
    {
        private const float _scanPeriod = 1f;

        [SerializeField] private LayerMask _mask;
        [SerializeField] private float _seekRadius;
        [SerializeField] private float _lostRadius;

        private float _scanTimer = 0;
        private Entity _self;
        private Entity _entity;
        public Entity GetEntity() => _entity;

        protected virtual bool IsSuatable(Entity entity)
        {
            return true;
        }

        private void Start()
        {
            _self = GetComponentInParent<Entity>();
        }

        private void Update()
        {
            if (_scanTimer > 0)
            {
                _scanTimer -= Time.deltaTime;
            }
            else
            {
                _scanTimer = _scanPeriod;

                Scan();
            }
        }

        private void Scan()
        {
            if (_entity)
            {
                float sqrDistance = (transform.position - _entity.transform.position).sqrMagnitude;

                if (sqrDistance > _lostRadius*_lostRadius)
                {
                    _entity = null;
                }
            }

            if (!_entity)
            {
                Collider[] colliders = Physics.OverlapSphere(transform.position, _seekRadius, _mask);

                foreach (var collider in colliders)
                {
                    Entity entity = collider.GetComponent<Entity>();

                    if (entity && entity != _self && IsSuatable(entity))
                    {
                        _entity = entity;
                        break;
                    }
                }
            }
        }


        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _seekRadius);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _lostRadius);
        }
    }
}