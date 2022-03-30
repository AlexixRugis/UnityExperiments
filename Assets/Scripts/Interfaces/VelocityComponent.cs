using UnityEngine;

namespace Mobs
{
    public abstract class VelocityComponent : MonoBehaviour
    {
        public Vector3 Velocity;
        private void Update()
        {
            ApplyVelocity();
        }

        protected abstract void ApplyVelocity();
    }
}
