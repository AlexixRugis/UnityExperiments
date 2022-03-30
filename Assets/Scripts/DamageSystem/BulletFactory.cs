using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mobs
{
    public static class BulletFactory
    {
        public static Bullet CreateBullet(Bullet bulletPrefab, Vector3 startPosition, Vector3 targetPosition, float speed)
        {
            Bullet bullet = Object.Instantiate(bulletPrefab, startPosition, Quaternion.identity, null);
            bullet.Initialize(startPosition, targetPosition, speed);

            return bullet;
        }
    }
}