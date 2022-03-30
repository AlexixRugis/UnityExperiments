using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mobs
{
    public class NefriteBossCreationStage : MonoBehaviour, IBossStage
    {
        [SerializeField] private GameObject _graphics;
        [Header("Close Zone")]
        [SerializeField] private GameObject _closeZonePrefab;
        [Header("Middle Zone")]
        [SerializeField] private GameObject _middleZonePrefab;
        [SerializeField] private int _middlePerSectorPrefabs;
        [Header("Aware Zone")]
        [SerializeField] private GameObject _awareZonePrefab;
        [SerializeField] private int _awarePrefabsCount;

        private NefriteBoss _boss;

        private bool _isCloseActive = false;
        private bool _isMiddleActive = false;
        private bool IsAwareActive = false;

        private void Start()
        {
            _boss = GetComponentInParent<NefriteBoss>();

            if (!_boss)
            {
                throw new System.Exception($"Can't find the instance of {_boss.GetType()}");
            }
        }

        public IEnumerator StartClose(Vector3 position)
        {
            if (!_isCloseActive)
            {
                _isCloseActive = true;
                List<GameObject> _spawnedPrefabs = new List<GameObject>();

                Vector3 bossToPosition = position - _boss.transform.position;

                Vector3 spawnOffset = bossToPosition.normalized * Mathf.Min(bossToPosition.magnitude, _boss.CloseZoneRadius);
                Vector3 spawnPosition = _boss.transform.position + spawnOffset;

                GameObject go = Instantiate(_closeZonePrefab, spawnPosition, Quaternion.LookRotation(bossToPosition, Vector3.up), transform);

                _spawnedPrefabs.Add(go);

                yield return new WaitForSeconds(1f);

                Destroy(go);

                yield return new WaitForSeconds(1f);

                _isCloseActive = false;
            }
            yield return null;
        }

        public IEnumerator StartMiddle(Vector3 position)
        {
            if (!_isMiddleActive)
            {
                _isMiddleActive = true;
                List<GameObject> _spawnedPrefabs = new List<GameObject>();

                float sectorAngle = GetSectorAngle(position);
                Debug.Log(sectorAngle);

                for (int i = 0; i < _middlePerSectorPrefabs; i++)
                {
                    Vector3 spawnOffset = Quaternion.Euler(0, sectorAngle + i * (90f / _middlePerSectorPrefabs), 0) * Vector3.forward * _boss.MiddleZoneRadius;
                    Vector3 spawnPosition = _boss.transform.position + spawnOffset;

                    GameObject go = Instantiate(_middleZonePrefab, spawnPosition, Quaternion.LookRotation(spawnOffset, Vector3.up), transform);

                    _spawnedPrefabs.Add(go);

                    yield return new WaitForSeconds(0.4f);
                }

                yield return new WaitForSeconds(1f);

                foreach (var go in _spawnedPrefabs)
                {
                    Destroy(go);
                }

                yield return new WaitForSeconds(1f);

                _isMiddleActive = false;

            }
            yield return null;
        }

        public IEnumerator StartAware()
        {
            if (!IsAwareActive)
            {
                IsAwareActive = true;

                List<GameObject> _spawnedPrefabs = new List<GameObject>();

                for (int i = 0; i < _awarePrefabsCount; i++)
                {
                    Vector3 spawnOffset = Quaternion.Euler(0, i * (360f / _awarePrefabsCount), 0) * Vector3.forward * _boss.AwareZoneRadius;
                    Vector3 spawnPosition = _boss.transform.position + spawnOffset;

                    GameObject go = Instantiate(_awareZonePrefab, spawnPosition, Quaternion.LookRotation(spawnOffset, Vector3.up), transform);

                    _spawnedPrefabs.Add(go);
                }

                yield return new WaitForSeconds(1f);

                foreach(var go in _spawnedPrefabs)
                {
                    Destroy(go);
                }

                yield return new WaitForSeconds(1f);

                IsAwareActive = false;
            }
            yield return null;
        }

        private float GetSectorAngle(Vector3 position)
        {
            Vector3 bossToPosition = position - _boss.transform.position;

            if (bossToPosition.x >= 0)
            {
                if (bossToPosition.z >= 0)
                {
                    return 0;
                }
                else
                {
                    return 90;
                }
            }
            else
            {
                if (bossToPosition.z >= 0)
                {
                    return 270;
                }
                else
                {
                    return 180;
                }
            }
        }

        public IEnumerator StartStage()
        {
            _graphics?.SetActive(true);
            yield return null;
        }

        public IEnumerator ProcessStage()
        {
            Vector3 player = _boss.GetPlayer().position;

            switch (_boss.GetPlayerZone())
            {
                case 1:
                    StartCoroutine(StartClose(player));
                    break;

                case 2:
                    StartCoroutine(StartMiddle(player));
                    break;

                case 3:
                    StartCoroutine(StartAware());
                    break;
            }

            yield return null;
        }

        public IEnumerator EndStage()
        {
            yield return null;
            _graphics?.SetActive(false);
        }
    }
}