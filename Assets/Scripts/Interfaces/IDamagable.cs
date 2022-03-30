using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mobs
{
    public interface IDamagable
    {
        void TakeDamage(int amount);
    }
}