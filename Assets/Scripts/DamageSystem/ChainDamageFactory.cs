using ARTech.GameFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mobs
{
    public static class ChainDamageFactory
    {
        public static void Spawn(ChainDamageEntity prefab, Vector3 from, Vector3 to, float maxRadius, int damage, int remainingDamagebles, List<ARTGF_IDamageable> damaged)
        {
            ChainDamageEntity damageEntity = Object.Instantiate(prefab, from, Quaternion.identity);

            damageEntity.Initialize(from, to, maxRadius, damage, remainingDamagebles, damaged);
        }
    }
}