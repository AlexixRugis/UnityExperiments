using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARTech.GameFramework
{
    public abstract class Projectile : Entity
    {
        private Entity _shooter;

        public Entity GetShooter() => _shooter;
        public void SetShooter(Entity shooter) => _shooter = shooter;
    }
}