using UnityEngine;
using System.Linq;

namespace Mobs
{
    public class AvoidTypes : EntityTriggerZone
    {
        [SerializeField] private AttackableType[] _types;
        protected override bool IsSuatable(Entity entity) => _types.Contains(entity.Type);
    }
}