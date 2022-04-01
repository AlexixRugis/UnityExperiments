using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mobs
{
    public class NefriteBossStrengthStage : MonoBehaviour, IBossStage
    {
        [SerializeField] private GameObject _graphics;

        private NefriteBoss _boss;

        [SerializeField] private StrengthWeaponRoot _weaponRoot;
        [SerializeField] private StrengthLaser[] _lasers;

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


                _weaponRoot.Target = null;

                foreach (var laser in _lasers)
                {
                    laser.Target = null;
                }

                _isCloseActive = false;
            }

            yield return null;
        }

        public IEnumerator StartMiddle()
        {
            if (!_isMiddleActive)
            {
                _isMiddleActive = true;

                _weaponRoot.Target = null;

                foreach (var laser in _lasers)
                {
                    laser.Target = null;
                }

                yield return new WaitForSeconds(1f);

                _isMiddleActive = false;
            }

            yield return null;
        }

        public IEnumerator StartAware(Transform target)
        {
            if (!_isAwareActive)
            {
                _isAwareActive = true;

                _weaponRoot.Target = target;
                
                foreach (var laser in _lasers)
                {
                    laser.Target = target;
                }

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
                    StartCoroutine(StartMiddle());
                    break;

                case 3:
                    StartCoroutine(StartAware(player));
                    break;
            }

            yield return new WaitForSeconds(1f);
        }

        public IEnumerator EndStage()
        {
            yield return null;
            _graphics?.SetActive(false);
        }
    }
}