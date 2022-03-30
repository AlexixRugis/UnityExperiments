using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mobs
{
    public sealed class LifeTimeComponent : MonoBehaviour
    {
        public float LifeTime;

        private float _startTime;

        private void Awake()
        {
            _startTime = Time.time;
        }

        private void Update()
        {
            if (Time.time >= _startTime + LifeTime) Destroy(gameObject);
        }
    }
}