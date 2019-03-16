using System;
using System.Collections;
using UnityEngine;

namespace Grandma.ParametricFirearms
{
    [RequireComponent(typeof(Moveable))]
    public class PFAI : Agent
    {
        public Damageable startingTarget;

        [Header("PF References")]
        public Transform pfParent;
        public ParametricFirearm pfPrefab;

        private Moveable moveable;
        private ParametricFirearm pf;
        private Transform referenceBarrelTransform;

        private Coroutine executing;

        public enum State
        {
            Idle, //Stationary
            Intercepting,
            Firing,
        }

        private State state;
        private State CurrentState
        {
            get
            {
                return state;
            }
            set
            {
                state = value;
                Debug.Log(this);
            }
        }


        protected override void Awake()
        {
            base.Awake();

            if (pfPrefab == null)
            {
                Debug.LogError("PF_AI: Prefab not assigned");
                return;
            }

            //Will change as Active is changed
            moveable = GetComponent<Moveable>();

            //PF Init
            pf = Instantiate(pfPrefab, pfParent).GetComponent<ParametricFirearm>();

            pf.transform.localPosition = Vector3.zero;
            pf.transform.localRotation = Quaternion.identity;
            pf.transform.localScale = Vector3.one;

            referenceBarrelTransform = new GameObject("Reference Barrel Transform").transform;

            //Should not move with the parent
            referenceBarrelTransform.SetParent(pfParent.parent);
            referenceBarrelTransform.transform.position = pf.barrelTip.transform.position;
            referenceBarrelTransform.transform.rotation = pf.barrelTip.transform.rotation;
        }

        protected override void Start()
        {
            base.Start();

            if (pf == null)
            {
                return;
            }

            //Movement Speed changes established
            pf.OnUpdated += (updatedComp) =>
            {
                var updatedPf = updatedComp as ParametricFirearm;

                if (updatedComp != null)
                {
                /*
                //State transitions to charging - firing begins
                if (state == ParametricFirearm.PFState.Charging)
                {
                    //Modify movement by firing move speed
                    agent.speed = Data.moveSpeed * Data.firingMoveSpeedFactor;
                }
                //State transitions to ready - firing finished or cancelled
                else if (state == ParametricFirearm.PFState.Ready)
                {
                    //Restore normal speed
                    agent.speed = Data.moveSpeed;
                }
                */
                }
            };

            if(startingTarget != null)
            {
                SetTarget(startingTarget);
            }
        }

        public void SetTarget(Damageable d)
        {
            if (pf == null)
            {
                Debug.LogError("PF_AI: PF is null");
                return;
            }

            if(executing != null)
            {
                StopCoroutine(executing);
            }

            executing = StartCoroutine(Intercept(d));
        }

        private IEnumerator Intercept(Damageable d)
        {
            CurrentState = State.Intercepting;

            moveable.ActiveController.CanMove = true;


            while (TargetInSight(d) == false || TargetInRange(d.transform) == false)
            {
                var vecMove = moveable.ActiveController as MoveToVector;

                if (vecMove != null)
                {
                    vecMove.Target = d.transform.position;
                }
                
                //Look at in Y Axis
                var lookPos = d.transform.position - transform.position;
                lookPos.y = 0;
                transform.rotation = Quaternion.LookRotation(lookPos);

                yield return null;
            }

            if (d != null)
            {
                //Target in sight, begin attack
                executing = StartCoroutine(Firing(d));
            }
            else
            {
                //case: Target Dead
                CurrentState = State.Idle;
            }
        }

        private IEnumerator Firing(Damageable d)
        {
            CurrentState = State.Firing;

            moveable.ActiveController.CanMove = false;

            var aim = StartCoroutine(Aim(d.transform));

            while (TargetInSight(d) && TargetInRange(d.transform))
            {
                if (pf.State == ParametricFirearm.PFState.Ready)
                {
                    pf.TriggerPress();

                }

                yield return null;

            }

            StopCoroutine(aim);

            //Target escaped case
            if (d != null)
            {
                StartCoroutine(Intercept(d));
            }
            else
            //Target dead case
            {
                CurrentState = State.Idle;
            }
        }

        private IEnumerator Aim(Transform d)
        {
            //Controlled by Firing
            while (true)
            {
                //Turn body towards planar point of target
                transform.LookAt(new Vector3(d.position.x, transform.position.y, d.position.z));

                if (TargetInRange(d))
                {
                    pfParent.localEulerAngles = new Vector3(-CalculatePFWeaponAngle(d), 0, 0);
                }

                yield return null;
            }
        }

        private bool TargetInSight(Damageable d)
        {
            if (d == null)
            {
                return false;
            }

            //Measured from pf barrel tip
            Vector3 startPos = referenceBarrelTransform.position;
            Vector3 endPos = d.transform.position;
            //float maxRange = CalculateMaxPFRange(d.transform);

            RaycastHit info;

            if (Physics.Raycast(startPos, (endPos - startPos), out info, Mathf.Infinity))
            {
                var d1 = info.collider.GetComponentInParent<Damageable>();

                return d1 == d;
            }

            return false;
        }

        private bool TargetInRange(Transform d)
        {
            return float.IsNaN(CalculatePFWeaponAngle(d)) == false;
        }


        /// <summary>
        /// Calculates the lowest firing angle to reach the target
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        private float CalculatePFWeaponAngle(Transform d)
        {
            //Initial velocity
            float v0 = pf.PFData.Trajectory.initialForceVector;
            //Gravity (acceleration)
            float g = (pf.PFData.Trajectory.dropOffRatio * v0) / Time.fixedDeltaTime;
            //Height difference
            float sy = referenceBarrelTransform.position.y - d.position.y;
            //Distance
            float sx = referenceBarrelTransform.position.x - d.position.x;
            float sz = referenceBarrelTransform.position.z - d.position.z;

            //planar distance
            float p = Mathf.Sqrt(sx * sx + sz * sz);

            float q = -((g * p * p) / (2 * v0 * v0));

            float tanTheata0 = (-p + Mathf.Sqrt((p * p) - (4 * q * (q + sy)))) / (2 * q);
            float tanTheata1 = (-p - Mathf.Sqrt((p * p) - (4 * q * (q + sy)))) / (2 * q);

            float theta0 = Mathf.Max(0, Mathf.Atan(tanTheata0)) * (180f / Mathf.PI);
            float theta1 = Mathf.Max(0, Mathf.Atan(tanTheata1)) * (180f / Mathf.PI);

            return Mathf.Min(theta0, theta1);
        }

        public override string ToString()
        {
            return string.Format("PFAI (ID: {0}) is now in state {1}", ObjectID, state);
        }
    }
}
