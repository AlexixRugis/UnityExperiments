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
        [SerializeField] private Projectile _projectilePrefab;
        [SerializeField] private float _bulletSpeed;
        [SerializeField] private float _bulletSpawnDuration;

        private ICharacter _host;
        private ICharacter _target;
        private Vector3 _lastTargetPosition;

        private void Awake()
        {
            _host = GetComponent<INPC>();
        }

        private void Update()
        {
            if (_target != null && _host.CanSee(_target))
            {
                _lastTargetPosition = _target.Position;
                _bulletHolder.LookAt(_lastTargetPosition);
            }
        }

        public void AttackRanged(ICharacter damageable)
        {
            StartCoroutine(Attack(damageable));
        }

        private IEnumerator Attack(ICharacter target)
        {
            _target = target;
            Projectile[] bullets = new Projectile[_bulletPositions.Length];
            var arr = _bulletPositions.OrderBy(x => Random.Range(0, _bulletPositions.Length)).ToArray();

            for (int i = 0; i < bullets.Length; i++)
            {
                Projectile bullet = Instantiate(_projectilePrefab, arr[i].transform.position, Quaternion.identity, _bulletHolder);
                bullets[i] = bullet;
                
                yield return new WaitForSeconds(_bulletSpawnDuration);
            }

            for (int i = 0; i < bullets.Length; i++)
            {
                Vector3 direction = _lastTargetPosition - bullets[i].transform.position;
                bullets[i].Speed = _bulletSpeed;
                bullets[i].DamageableTypesPredicate = d => d is not JadeGoastWispBT;
                bullets[i].Launch(direction);
                bullets[i].transform.SetParent(null);
                yield return new WaitForSeconds(_bulletSpawnDuration);
            }

            yield return null;
        }
    }
}