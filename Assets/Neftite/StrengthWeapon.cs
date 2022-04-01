using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mobs
{
    public class StrengthWeapon : MonoBehaviour
    {
        [SerializeField] private float _rotationSpeed;

        private void Update()
        {
            transform.Rotate(_rotationSpeed * Time.deltaTime, 0, 0);
        }
    }
}