using ARTech.GameFramework;
using System.Collections;
using UnityEngine;

namespace Mobs
{
    public sealed class SummonerAttackHandler : ARTGF_AttackHandler
    {
        [SerializeField] private ARTGF_WeaponHitbox spawnPrefab;
        [SerializeField] private float spawnCount;
        [SerializeField] private float spawnDistance;
        [SerializeField] private float spawnDuration;

        protected override IEnumerator Perform(ARTGF_Character target)
        {
            IsPerforming = true;

            yield return new WaitForSeconds(1f);

            Vector3 direction = (target.transform.position - transform.position).normalized;

            float currentDistance = spawnDistance;

            for (int i = 0; i < spawnCount; i++)
            {
                Vector3 spawnPoint = transform.position + direction * currentDistance;
                ARTGF_WeaponHitbox weapon = Instantiate(spawnPrefab, spawnPoint, Quaternion.identity);
                weapon.DamageablePredicate = c => c is ARTGF_Player;

                yield return new WaitForSeconds(spawnDuration);

                currentDistance += spawnDistance;
            }

            IsPerforming = false;
        }
    }
}