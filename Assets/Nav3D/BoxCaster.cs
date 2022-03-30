using System;
using UnityEngine;


namespace ARTech.Nav3D
{
    [Serializable]
    public class BoxCaster
    {
        public LayerMask LayerMask;
        public float Size;

        public float HalfSize => Size * 0.5f;

        public bool Check(Vector3 position)
        {
            return Physics.CheckBox(position, Vector3.one * HalfSize, Quaternion.identity, LayerMask);
        }
    }
}