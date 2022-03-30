using System.Linq;
using UnityEngine;

namespace Mobs
{
    public class AttackTypes : EntityTriggerZone
    {
        [SerializeField] private AttackableType[] _types;
        protected override bool IsSuatable(Entity entity) => _types.Contains(entity.Type);
    }
}