using UnityEngine;

namespace ARTech.GameFramework
{
    public class Spawnpoint : MonoBehaviour
    {
        [SerializeField] private Color debugColor = new Color(1f, 0f, 1f, 0.5f);

        public Vector3 Position => transform.position;

        private void OnDrawGizmos()
        {
            Gizmos.color = debugColor;
            Gizmos.DrawSphere(Position, 1f);
        }
    }
}