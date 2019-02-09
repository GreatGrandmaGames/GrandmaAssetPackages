using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using Grandma;
using Grandma.Utility;

namespace Grandma.ParametricFirearms
{
    //TODO: make use of Moveable here
    [RequireComponent(typeof(Rigidbody))]
    public class PFProjectile : GrandmaComponent
    {
        [Header("Projectile Options")]
        public float cleanUpTime = 5f;

        //Calculated properties
        private float timeSinceCreation = 0.0f;
        private float distanceTravelled = 0.0f;
        private Vector3? previousPosition = null; //used for previousPosition
        private int numberOfCollisions; //used for explosion

        //Component References
        Rigidbody rb;

        private PFProjectileData projData;

        private Agent firingAgent;
        private ParametricFirearm firingPF;

        protected override void Awake()
        {
            base.Awake();

            rb = GetComponent<Rigidbody>();
            
            //Clean up
            Destroy(gameObject, cleanUpTime);
        }

        public void Launch(Agent agent, ParametricFirearm pf, PFProjectileData data)
        {
            this.firingAgent = agent;
            this.firingPF = pf;

            this.Read(data);

            ApplyStartingTrajectory();
        }

        protected override void OnRead(GrandmaComponentData data)
        {
            base.OnRead(data);

            projData = data as PFProjectileData;
        }

        private void FixedUpdate()
        {
            if (projData == null)
            {
                //not ready to start moving
                return;
            }

            //Motion
            AddAdditionalForces(timeSinceCreation);

            //Frame admin
            //Record travel time
            timeSinceCreation += Time.fixedDeltaTime;

            //Record distance travelled
            if (previousPosition.HasValue)
            {
                distanceTravelled += Vector3.Distance(previousPosition.Value, transform.position);
            }

            previousPosition = transform.position;
        }

        private void OnCollisionEnter(Collision collision)
        {
            numberOfCollisions++;

            var hitMax = numberOfCollisions >= projData.AreaDamage.numImpactsToDetonate;
            var damageable = collision.transform.GetComponent<Damageable>();

            if(damageable != null)
            {
                Impact(damageable);
                Explode();
            }
            else if (hitMax)
            {
                Explode();
            }
        }

        #region Motion
        private void ApplyStartingTrajectory()
        {
            //Generate any projectile specific random values
            float trajectoryScalarX = RandomUtility.RandFloat(-1f, 1f);
            float trajectoryScalarY = RandomUtility.RandFloat(-1f, 1f);

            Vector3 v = transform.forward;

            v += transform.right * Mathf.Tan(trajectoryScalarX) * projData.Trajectory.maxInitialSpreadAngle;
            v += transform.up * Mathf.Tan(trajectoryScalarY) * projData.Trajectory.maxInitialSpreadAngle;

            v = v.normalized * projData.Trajectory.initialForceVector;

            rb.AddForce(v, ForceMode.Impulse);
        }

        private void AddAdditionalForces(float time)
        {
            //Gravity
            //WHy do we multiply by initialSpeed here?
            //If we just applied some drop off force, this would be a NON-ORTHONGONAL feature
            //because the faster your initial speed, the less dropoff has time to affect the course 
            //of the projetile.
            //Here, the initial speed is included so that no matter how fast the projectile travels,
            //the same dropoff ratio for one gun is the same for another (with a different initial speed)
            rb.AddForce(Vector3.down * projData.Trajectory.dropOffRatio * projData.Trajectory.initialForceVector / Time.fixedDeltaTime);

            //Bullet curve, heat seeking etc.
        }

        #endregion

        #region Impact
        /// <summary>
        /// When a projectile impacts some surface
        /// </summary>
        private void Impact(Damageable damagable)
        { 
            if(damagable == null)
            {
                return;
            }

            damagable.Damage(new DamageablePayload(firingAgent.ObjectID, firingPF.ObjectID, CalculateDamageOnImpact()));
        }

        /// <summary>
        /// Uses PGFImpaceDamageData to calculate impace damage. This is non-explosive damage
        /// e.g. piercing, blunt force
        /// </summary>
        /// <returns></returns>
        public float CalculateDamageOnImpact()
        {
            return projData.ImpactDamage.baseDamage + projData.ImpactDamage.damageChangeByDistance * distanceTravelled;
        }
        #endregion

        #region Area
        public void Explode()
        {
            if(projData.AreaDamage.explodable == false)
            {
                return;
            }

            foreach (var col in Physics.OverlapSphere(transform.position, projData.AreaDamage.maxBlastRange))
            {
                var dam = col.GetComponent<Damageable>();

                if (dam != null)
                {
                    dam.Damage(new DamageablePayload(firingAgent.ObjectID, firingPF.ObjectID, CalculateDamageOnExplosion(dam.transform.position)));
                }
            }

            Destroy(gameObject);
        }

        private float CalculateDamageOnExplosion(Vector3 otherPosition)
        {
            float dist = Vector3.Distance(transform.position, otherPosition);

            if (dist < projData.AreaDamage.maxBlastRange)
            {
                return (dist / projData.AreaDamage.maxBlastRange) * projData.AreaDamage.maxDamage;
            }
            else
            {
                return 0f;
            }
        }
        #endregion
    }
}
