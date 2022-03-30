using System.Collections.Generic;
using UnityEngine;

namespace Mobs
{
    public enum AttackableType
    {
        Plant,
        Insect,
        Animal,
        Player
    }

    public class AttackableComponent : MonoBehaviour
    {
        [SerializeField] private AttackableType _type;

        public readonly List<AttackerComponent> Attackers = new List<AttackerComponent>();
        public AttackableType Type => _type;

        public AttackerComponent GetNearestAttacker()
        {
            float minDistanceSqr = float.PositiveInfinity;
            AttackerComponent nearest = null;

            for (int i = 0; i < Attackers.Count; i++)
            {
                float sqrDistance = (Attackers[i].transform.position - transform.position).sqrMagnitude;
                if (sqrDistance < minDistanceSqr)
                {
                    minDistanceSqr = sqrDistance;
                    nearest = Attackers[i];
                }
            }

            return nearest;
        }
    }
}