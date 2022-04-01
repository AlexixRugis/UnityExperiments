using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mobs
{
    public class StrengthWeaponRoot : MonoBehaviour
    {
        public Transform Target;

        private void Update()
        {
            if (Target)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Target.position - transform.position), 10f * Time.deltaTime);
            }
            else
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(90, 0, 0), Time.deltaTime);
            }
        }
    }
}