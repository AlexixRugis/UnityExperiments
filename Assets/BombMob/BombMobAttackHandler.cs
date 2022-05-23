using ARTech.GameFramework;
using System.Collections;
using UnityEngine;

namespace Mobs
{
    public sealed class BombMobAttackHandler : AttackHandler
    {
        [SerializeField] private Transform bombHolder;
        [SerializeField] private Bomb bombPrefab;
        [SerializeField] private float throwAngle;

        protected override IEnumerator Perform(Character target)
        {
            IsPerforming = true;

            Bomb bomb = Instantiate(bombPrefab, bombHolder.position, Quaternion.identity, bombHolder);

            yield return new WaitForSeconds(1.5f);

            Vector3 direction = target.transform.position - bomb.transform.position;
            float y = direction.y;
            direction.y = 0;
            float x = direction.magnitude;

            float a = Mathf.Deg2Rad * throwAngle;
            float sqrV = (-9.8f * x * x) / (2 * (y - Mathf.Tan(a) * x) * Mathf.Pow(Mathf.Cos(a), 2));
            float v = Mathf.Sqrt(sqrV);
            bomb.Speed = v;

            Vector3 throwDirection = direction + Vector3.up * (x * Mathf.Tan(a));

            bomb.transform.SetParent(null);
            bomb.Launch(throwDirection);

            yield return new WaitForSeconds(1f);

            IsPerforming = false;
        }
    }
}