using UnityEngine;

namespace Mobs
{
    [RequireComponent(typeof(AvoidTypes))]
    public sealed class RunAwayFSMComponent : FSMComponent
    {
        public float ChaseTime;
        public StateComponent PatrolState;
        public StateComponent RunAwayState;

        private AvoidTypes _avoidTypes;
        private float _chaseStartTime;

        private void Awake()
        {
            _avoidTypes = GetComponent<AvoidTypes>();

            if (PatrolState) RegisterState(PatrolState);
            if (RunAwayState) RegisterState(RunAwayState);
        }

        protected override void Update()
        {
            base.Update();

            if (_avoidTypes.GetEntity()) _chaseStartTime = Time.time;

            if (_chaseStartTime + ChaseTime < Time.time)
            {
                if (CurrentState != PatrolState)
                {
                    Debug.Log("patrol");
                    TransitToState(PatrolState);
                }
            }
            else
            {
                if (CurrentState != RunAwayState)
                {
                    Debug.Log("run");
                    TransitToState(RunAwayState);
                }
            }
        }
    }
}