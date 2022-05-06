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
        [SerializeField] private Character _prefab;
        [SerializeField] private int _maxSpawnCount;
        [SerializeField] private int _spawnDuration;
        [SerializeField] private Transform[] _spawnpoints;

        private Collider _collider;
        private List<Character> _npcs = new List<Character>();

        public IEnumerable<Vector3> Spawnpoints => _spawnpoints.Select(t => t.position);

        public IEnumerable<Character> NPCs => _npcs;

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

        private void Spawn(Character prefab, Vector3 spawnpoint)
        {
            Character npc = Instantiate(prefab, spawnpoint, Quaternion.identity);
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

        public float GetDistance(Vector3 from)
        {
            return Vector3.Distance(_collider.ClosestPoint(from), from);
        }

        public Vector3 GetClosestPoint(Vector3 to)
        {
            return _collider.ClosestPoint(to);
        }

        public Vector3 GetRandomPointIn()
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