using ARTech.GameFramework;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Mobs
{
    [RequireComponent(typeof(JadeGoastWispBT))]
    public class JadeGoastAttackHandler : MonoBehaviour, IRangedAttackHandler
    {
        [SerializeField] private Transform _bulletHolder;
        [SerializeField] private Transform[] _bulletPositions;
        [SerializeField] private Bullet _bulletPrefab;
        [SerializeField] private float _bulletSpeed;
        [SerializeField] private float _bulletSpawnDuration;

        private LivingEntity _host;
        private LivingEntity _target;
        private Vector3 _lastTargetPosition;

        private void Awake()
        {
            _host = GetComponent<LivingEntity>();
        }

        private void Update()
        {
            if (_target != null && _host.CanSee(_target))
            {
                _lastTargetPosition = _target.GetLocation();
                _bulletHolder.LookAt(_lastTargetPosition);
            }
        }

        public void AttackRanged(LivingEntity damageable)
        {
            StartCoroutine(Attack(damageable));
        }

        private IEnumerator Attack(LivingEntity target)
        {
            _target = target;
            Bullet[] bullets = new Bullet[_bulletPositions.Length];
            var arr = _bulletPositions.OrderBy(x => Random.Range(0, _bulletPositions.Length)).ToArray();

            for (int i = 0; i < bullets.Length; i++)
            {
                Bullet bullet = Instantiate(_bulletPrefab, arr[i].transform.position, Quaternion.identity, _bulletHolder);
                bullets[i] = bullet;

                yield return new WaitForSeconds(_bulletSpawnDuration);
            }

            for (int i = 0; i < bullets.Length; i++)
            {
                Vector3 targetPoint = _lastTargetPosition;
                bullets[i].Initialize(arr[i].transform.position, targetPoint, _bulletSpeed);
                bullets[i].transform.SetParent(null);
                yield return new WaitForSeconds(_bulletSpawnDuration);
            }

            yield return null;
        }
    }
}