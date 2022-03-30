using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mobs
{
    public class StrengthLaser : MonoBehaviour
    {
        public Transform Target;

        [SerializeField] private float _movementSmoothness;

        private void Update()
        {
            if (Target)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Target.position - transform.position), _movementSmoothness * Time.deltaTime);
            }
            else
            {
                transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(0, 180, 0), _movementSmoothness * Time.deltaTime);
            }
            
        }
    }
}