using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mobs
{
    public interface IBossStage
    {
        IEnumerator StartStage();
        IEnumerator ProcessStage();
        IEnumerator EndStage();
    }
}