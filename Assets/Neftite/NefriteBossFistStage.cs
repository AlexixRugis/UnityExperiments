using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Mobs
{
    public class NefriteBossFistStage : MonoBehaviour, IBossStage
    {
        [SerializeField] private GameObject _graphics;

        [Header("Aware settings")]
        [SerializeField] private GameObject _awarePrefab;
        [SerializeField] private int _awarePlacesCount;
        [SerializeField] private int _awareCount;
        [SerializeField] private float _betweenAwareTime;

        private NefriteBoss _boss;

        private bool _isCloseActive = false;
        private bool _isMiddleActive = false;
        private bool _isAwareActive = false;

        private void Awake()
        {
            _boss = GetComponentInParent<NefriteBoss>();

            if (!_boss)
            {
                throw new System.Exception($"Can't find the instance of {_boss.GetType()}");
            }
        }

        public IEnumerator StartClose()
        {
            if (!_isCloseActive)
            {
                _isCloseActive = true;

                Debug.Log("Fist close");

                _isCloseActive = false;
            }

            yield return null;
        }

        public IEnumerator StartMiddle()
        {
            if (!_isMiddleActive)
            {
                _isMiddleActive = true;

                Debug.Log("Fist middle");

                _isMiddleActive = false;
            }

            yield return null;
        }

        public IEnumerator StartAware()
        {
            if (!_isAwareActive)
            {
                _isAwareActive = true;

                List<GameObject> _spawnedPrefabs = new List<GameObject>();

                int spawnCount = Mathf.Min(_awarePlacesCount, _awareCount);
                List<int> spawnedIndices = new List<int>();

                for (int i = 0; i < spawnCount; i++)
                {
                    int randomIndex;
                    while (true)
                    {
                        randomIndex = Random.Range(0, _awarePlacesCount);
                        if (!spawnedIndices.Contains(randomIndex)) {
                            spawnedIndices.Add(randomIndex);
                            break;
                        }
                    }

                    Vector3 spawnOffset = Quaternion.Euler(0, randomIndex * (360f / _awarePlacesCount), 0) * Vector3.forward * _boss.AwareZoneRadius;
                    Vector3 spawnPosition = _boss.transform.position + spawnOffset;

                    GameObject go = Instantiate(_awarePrefab, spawnPosition, Quaternion.LookRotation(spawnOffset, Vector3.up), transform);

                    _spawnedPrefabs.Add(go);

                    yield return new WaitForSeconds(_betweenAwareTime);
                }

                yield return new WaitForSeconds(1f);

                foreach (var go in _spawnedPrefabs)
                {
                    Destroy(go);
                }

                yield return new WaitForSeconds(1f);

                _isAwareActive = false;
            }
            yield return null;
        }

        public IEnumerator StartStage()
        {
            _graphics?.SetActive(true);
            yield return null;
        }

        public IEnumerator ProcessStage()
        {
            Vector3 position = _boss.GetPlayer().position;

            switch (_boss.GetPlayerZone())
            {
                case 1:
                    StartCoroutine(StartClose());
                    break;

                case 2:
                    StartCoroutine(StartMiddle());
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