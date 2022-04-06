using System;
using System.Collections.Generic;
using UnityEngine;

namespace ARTech.GameFramework
{
    public abstract class Entity : MonoBehaviour, IEntity
    {
        public readonly Vector3 DirectionDown = Vector3.down;

        [SerializeField] private bool _debug = true;

        [SerializeField] private LayerMask _obstacleMask;
        [SerializeField] private LayerMask _entitiesMask;
        [SerializeField] private float _groundCheckDistance;
        [SerializeField] private float _dieTime;

        private bool _isRemoved = false;

        public bool DebugEnabled => _debug;
        public string Name { get => name; set => name = value; }
        public Vector3 Position { get => !IsRemoved ? transform.position : Vector3.zero; set { if(!IsRemoved) transform.position = value; } }
        public Quaternion Rotation { get => !IsRemoved ? transform.rotation : Quaternion.identity; set { if (!IsRemoved) transform.rotation = value; } }
        public float DieTime => throw new NotImplementedException();
        public bool IsRemoved => _isRemoved;

        protected virtual void Update()
        {
            if (!IsRemoved)
            {
                OnLifeUpdate();
            }
        }

        protected abstract void OnLifeUpdate();

        public IEnumerable<IEntity> GetNearbyEntities(float radius, Type[] entityTypes)
        {
            Collider[] colliders = Physics.OverlapSphere(Position, radius, _entitiesMask);
            var entities = new List<IEntity>(colliders.Length);
            foreach (var collider in colliders)
            {
                var entity = collider.GetComponent<IEntity>();
                if (entity != null)
                {
                    foreach (var type in entityTypes)
                    {
                        if (type.IsAssignableFrom(entity.GetType()))
                        {
                            entities.Add(entity);
                            break;
                        }
                    }
                }
            }

            return entities;
        }

        public IEntity GetNearest(float radius, Type[] entityTypes)
        {
            float minSqrDistance = float.PositiveInfinity;
            IEntity entity = null;

            foreach (var e in GetNearbyEntities(radius, entityTypes))
            {
                float sqrDistance = (Position - e.Position).sqrMagnitude;
                if (sqrDistance < minSqrDistance)
                {
                    minSqrDistance = sqrDistance;
                    entity = e;
                }

                break;
            }

            return entity;
        }

        public void Remove()
        {
            if (_isRemoved) return;

            _isRemoved = true;
            Destroy(gameObject, _dieTime);
        }

        protected virtual void OnDrawGizmosSelected()
        {
            if (_debug)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(Position, Position + DirectionDown * _groundCheckDistance);
            }
        }
    }
}