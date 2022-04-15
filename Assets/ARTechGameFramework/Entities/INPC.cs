using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARTech.GameFramework
{
    public interface INPC : ICharacter
    {
        IArea Area { get; set; }
        ICharacter BattleTarget { get; set; }
        float LastTargetSeeTime { get; }

        Vector3? GetRandomPositionAround(float radius);
        Vector3? GetPositionFrom(Vector3 from, float radius);
    }
}