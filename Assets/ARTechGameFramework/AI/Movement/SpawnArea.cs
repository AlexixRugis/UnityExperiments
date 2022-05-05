using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ARTech.GameFramework
{
    [RequireComponent(typeof(Collider))]
    public class SpawnArea : MonoBehaviour
    {
        [SerializeField] private LayerMask _obstacleMask;
        [SerializeField] private AICharacter _prefab;
        [SerializeField] private int _maxSpawnCount;
        [SerializeField] private int _spawnDuration;
        [SerializeField] private Transform[] _spawnpoints;

        private Collider _collider;
        private List<AICharacter> _npcs = new List<AICharacter>();

        public IEnumerable<Vector3> Spawnpoints => _spawnpoints.Select(t => t.position);

        public IEnumerable<AICharacter> NPCs => _npcs;

        public int SpawnDuration { get => _spawnDuration; set => _spawnDuration = value; }
        public int MaxSpawnCount { get => _maxSpawnCount; set => _maxSpawnCount = value; }

        private void Awake()
        {
            _collider = GetComponent<Collider>();
        }

        private void Start()
        {
            StartCoroutine(SpawnRountine());
        }

        private void Spawn(AICharacter prefab, Vector3 spawnpoint)
        {
            AICharacter npc = Instantiate(prefab, spawnpoint, Quaternion.identity);
            npc.Area = this;

            _npcs.Add(npc);
        }

        private IEnumerator SpawnRountine()
        {
            while (true)
            {
                _npcs.RemoveAll(n => !n.IsAlive);

                if (_npcs.Count < MaxSpawnCount)
                {
                    Spawn(_prefab, _spawnpoints[Random.Range(0, _spawnpoints.Length)].position);
                }

                yield return new WaitForSeconds(SpawnDuration);
            }
        }

        public Vector3? GetRandomPointAround(Vector3 center, float maxDistance, float agentRadius)
        {
            Vector3 direction = Random.onUnitSphere * maxDistance;
            Vector3 directionNormalized = direction.normalized;

            if (!Physics.SphereCast(transform.position, agentRadius, directionNormalized, out RaycastHit hit, maxDistance, _obstacleMask))
            {
                Vector3 position = _collider.ClosestPoint(center + direction);

                if (Vector3.Distance(position, center) >  maxDistance)
                {
                    return null;
                }

                return position;
            }

            return null;
        }

        public float GetDistance(Vector3 from)
        {
            return Vector3.Distance(_collider.ClosestPoint(from), from);
        }

        public Vector3? GetRandomPointIn()
        {
            Vector3 position = _collider.ClosestPoint(
                new Vector3(
                    Random.Range(_collider.bounds.min.x, _collider.bounds.max.x),
                    Random.Range(_collider.bounds.min.y, _collider.bounds.max.y),
                    Random.Range(_collider.bounds.min.z, _collider.bounds.max.z)
                )
            );

            return position;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            foreach (var spawnpoint in Spawnpoints)
            {
                Gizmos.DrawCube(spawnpoint, Vector3.one * 0.2f);
            }
        }
    }
}