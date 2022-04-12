using System;
using System.Collections.Generic;
using UnityEngine;

namespace ARTech.GameFramework
{
    public abstract class Entity : TransformableObject, IEntity
    {
        public readonly Vector3 DirectionDown = Vector3.down;

        [SerializeField] private bool _debug = true;
        public bool DebugEnabled => _debug;

        protected virtual void Update()
        {
            if (!IsRemoved)
            {
                OnLifeUpdate();
            }
        }

        protected abstract void OnLifeUpdate();

        public IEnumerable<IEntity> GetNearbyEntities(float radius, Predicate<IEntity> match)
        {
            Collider[] colliders = Physics.OverlapSphere(Position, radius);
            var entities = new List<IEntity>(colliders.Length);
            foreach (var collider in colliders)
            {
                var entity = collider.GetComponent<IEntity>();
                if (entity != null && match.Invoke(entity))
                {
                    entities.Add(entity);
                }
            }

            return entities;
        }

        public IEntity GetNearest(float radius, Predicate<IEntity> match)
        {
            float minSqrDistance = float.PositiveInfinity;
            IEntity entity = null;

            foreach (var e in GetNearbyEntities(radius, match))
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

        protected virtual void OnDrawGizmosSelected()
        {
        }
    }
}