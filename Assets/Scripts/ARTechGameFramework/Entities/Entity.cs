using System;
using System.Collections.Generic;
using UnityEngine;

namespace ARTech.GameFramework
{
    public abstract class Entity : MonoBehaviour
    {
        public readonly Vector3 DirectionDown = Vector3.down;

        [SerializeField] private bool _debug = true;

        [SerializeField] private LayerMask _obstacleMask;
        [SerializeField] private LayerMask _entitiesMask;
        [SerializeField] private float _groundCheckDistance;
        [SerializeField] private float _dieTime;

        private bool _isRemoved = false;

        public bool DebugEnabled => _debug;
        public void SetName(string name) => this.name = name;
        public string GetName() => name;
        public Vector3 GetLocation() => transform.position;
        public void SetLocation(Vector3 location) => transform.position = location;

        public float GetDistanceToGround()
        {
            if (Physics.Raycast(transform.position, DirectionDown, out RaycastHit hit, Mathf.Infinity, _obstacleMask))
            {
                return hit.distance;
            }

            return Mathf.Infinity;
        }
        public bool IsOnGround() => GetDistanceToGround() > 0;

        public void Teleport(Vector3 position)
        {
            SetLocation(position);
        }

        public void Teleport(Entity other)
        {
            SetLocation(other.GetLocation());
        }

        public List<Entity> GetNearbyEntities(float radius)
        {
            Collider[] colliders = Physics.OverlapSphere(GetLocation(), radius, _entitiesMask);
            var entities = new List<Entity>(colliders.Length);
            foreach (var collider in colliders)
            {
                var entity = collider.GetComponent<Entity>();
                if (entity) entities.Add(entity);
            }

            return entities;
        }

        public Entity GetNearest(float radius, Type[] entityTypes)
        {
            float minSqrDistance = float.PositiveInfinity;
            Entity entity = null;

            foreach (var e in GetNearbyEntities(radius))
            {
                foreach (var type in entityTypes)
                {
                    if (type.IsAssignableFrom(e.GetType()))
                    {
                        float sqrDistance = (GetLocation() - e.GetLocation()).sqrMagnitude;
                        if (sqrDistance < minSqrDistance)
                        {
                            minSqrDistance = sqrDistance;
                            entity = e;
                        }

                        break;
                    }
                }
            }

            return entity;
        }

        public void Remove()
        {
            if (_isRemoved) return;

            _isRemoved = true;
            Destroy(gameObject, _dieTime);
        }

        public bool IsRemoved() => _isRemoved;

        protected virtual void OnDrawGizmosSelected()
        {
            if (_debug)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(GetLocation(), GetLocation() + DirectionDown * _groundCheckDistance);
            }
        }
    }
}