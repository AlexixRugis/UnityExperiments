using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mobs
{
    public class NefriteBossTruthStage : MonoBehaviour, IBossStage
    {
        [SerializeField] private GameObject _graphics;

        [Header("Middle settings")]
        [SerializeField] private GameObject _middlePrefab;
        [SerializeField] private int _middleCount;
        [SerializeField] private float _middleDistance;

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

                Debug.Log("Удар мечом ближний!");
                yield return new WaitForSeconds(1f);
                _isCloseActive = false;
            }

            yield return null;
        }

        public IEnumerator StartMiddle(Transform player)
        {
            if (!_isMiddleActive)
            {
                _isMiddleActive = true;

                yield return new WaitForSeconds(0.5f);

                List<GameObject> spawnedPrefabs = new List<GameObject>();

                Vector3 direction = (player.position - transform.position).normalized;
                float length = _middleDistance;

                for (int i = 0; i < _middleCount; i++)
                {
                    GameObject instance = Instantiate(_middlePrefab, transform.position + direction * length, Quaternion.LookRotation(direction), transform);
                    spawnedPrefabs.Add(instance);

                    length += _middleDistance;

                    yield return new WaitForSeconds(0.2f);
                }

                yield return new WaitForSeconds(1f);

                foreach (var instance in spawnedPrefabs)
                {
                    Destroy(instance);
                }

                _isMiddleActive = false;
            }

            yield return null;
        }

        public IEnumerator StartAware()
        {
            if (!_isAwareActive)
            {
                _isAwareActive = true;

                Debug.Log("Блок!");
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
            Transform player = _boss.GetPlayer();

            switch (_boss.GetPlayerZone())
            {
                case 1:
                    StartCoroutine(StartClose());
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