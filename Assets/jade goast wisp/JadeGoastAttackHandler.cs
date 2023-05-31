using ARTech.GameFramework;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Mobs
{
    [RequireComponent(typeof(JadeGoastWispBT))]
    public sealed class JadeGoastAttackHandler : ARTGF_AttackHandler
    {
        [SerializeField] private Transform _bulletHolder;
        [SerializeField] private Transform[] _bulletPositions;
        [SerializeField] private ARTGF_Projectile _projectilePrefab;
        [SerializeField] private float _bulletSpeed;
        [SerializeField] private float _bulletSpawnDuration;

        private ARTGF_Character _host;
        private ARTGF_Character _target;
        private Vector3 _lastTargetPosition;

        private void Awake()
        {
            _host = GetComponent<ARTGF_Character>();
        }

        private void Update()
        {
            if (_target != null && ARTGF_Utils.CanSee(_host.transform.position, _target, float.MaxValue))
            {
                _lastTargetPosition = _target.transform.position;
                _bulletHolder.LookAt(_lastTargetPosition);
            }
        }

        protected override IEnumerator Perform(ARTGF_Character target)
        {
            IsPerforming = true;
            _target = target;
            ARTGF_Projectile[] bullets = new ARTGF_Projectile[_bulletPositions.Length];
            var arr = _bulletPositions.OrderBy(x => Random.Range(0, _bulletPositions.Length)).ToArray();

            for (int i = 0; i < bullets.Length; i++)
            {
                ARTGF_Projectile bullet = Instantiate(_projectilePrefab, arr[i].transform.position, Quaternion.identity, _bulletHolder);
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
            IsPerforming = false;
            _target = null;
        }
    }
}