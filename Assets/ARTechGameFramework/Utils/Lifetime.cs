using UnityEngine;

namespace ARTech.GameFramework
{
    public sealed class Lifetime : MonoBehaviour
    {
        [SerializeField] private float lifetime;

        private void Start()
        {
            Destroy(gameObject, lifetime);
        }
    }
}