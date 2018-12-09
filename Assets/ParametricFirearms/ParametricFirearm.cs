using System;
using System.Collections;
using UnityEngine;

using Grandma.Core;

namespace Grandma.ParametricFirearms
{
    public class ParametricFirearm : GrandmaComponent
    {
        public PFProjectile projectilePrefab;
        [Tooltip("Where the projectile will spawn from and its initial direction (z-axis)")]
        public Transform barrelTip;

        //Data Properties
        [NonSerialized]
        private PFData pfData;

        //Ammo remaining in the clip
        public int CurrentAmmo { get; private set; }

        #region State Management
        public enum PFState
        {
            Ready,
            Charging,
            CoolDown,
            CoolDownInterupt,
            ManualReload
        }

        private PFState state;

        public PFState State
        {
            get
            {
                return state;
            }
            private set
            {
                state = value;

                if (onStateChanged != null)
                {
                    onStateChanged(value);
                }
            }
        }

        public Action<PFState> onStateChanged;

        private Coroutine chargeCoroutine;
        private Coroutine manaualReloadCoroutine;
        private Coroutine coolDownCoroutine;
        #endregion

        public override void Read(GrandmaComponentData data)
        {
            base.Read(data);

            pfData = data as PFData;

            CurrentAmmo = pfData.RateOfFire.AmmoCapacity;
        }

        #region Public Weapon Methods
        /// <summary>
        /// When in Ready state, will begin charging the weapon. NB if chargeTime is 0, will immediately call fire
        /// </summary>
        public Coroutine TriggerPress()
        {
            if (State == PFState.Ready)
            {
                State = PFState.Charging;
                chargeCoroutine = StartCoroutine(Charge());

                return chargeCoroutine;
            }

            return null;
        }

        /// <summary>
        /// If in Charging state, will either stop charging or fire depending on Data
        /// </summary>
        public Coroutine TriggerRelease()
        {
            if (State == PFState.Charging)
            {
                //Interupt charging
                StopCoroutine(chargeCoroutine);

                if (pfData.ChargeTime.requireFullyCharged == false)
                {
                    //Fire
                    return Fire();
                }
                else
                {
                    //Cancel
                    State = PFState.Ready;
                }
            }

            return null;
        }

        /// <summary>
        /// If in Ready or Charging, will begin a manual reload
        /// </summary>
        public void ManualReload()
        {
            if (State == PFState.Ready || State == PFState.Charging)
            {
                if (chargeCoroutine != null)
                {
                    StopCoroutine(chargeCoroutine);
                }

                manaualReloadCoroutine = StartCoroutine(Reload());
            }
        }

        /// <summary>
        /// If ManualReload, will switch back to ready
        /// </summary>
        public void CancelManualReload()
        {
            StopCoroutine(manaualReloadCoroutine);
            State = PFState.Ready;
        }

        public void ResumeCoolDown()
        {
            if (State == PFState.CoolDownInterupt)
            {
                State = PFState.CoolDown;
                coolDownCoroutine = StartCoroutine(CoolDown());
            }
        }

        public void InteruptCoolDown()
        {
            if (State == PFState.CoolDown)
            {
                State = PFState.CoolDownInterupt;
                StopCoroutine(coolDownCoroutine);
            }
        }
        #endregion

        #region Private Weapon Methods
        /// <summary>
        /// Launches projectile(s) and transistions into cool down
        /// </summary>    
        private Coroutine Fire()
        {
            if(projectilePrefab == null)
            {
                Debug.LogWarning("ParametricFirearm: Unable to fire as projectile prefab is null");
            }

            for (int i = 0; i < pfData.Multishot.numberOfShots; i++)
            {
                //Spawn the projectile
                var projectile = Instantiate(projectilePrefab);

                if(barrelTip != null)
                {
                    projectile.transform.position = barrelTip.position;
                    projectile.transform.forward = barrelTip.forward;
                }

                //Clone projectile data
                var projData = JsonUtility.FromJson<PFProjectileData>(JsonUtility.ToJson(pfData.Projectile));
                projectile.Launch(projData);

                //Controlling ROF
                //CUrrent ammo is decremented before being sent to GetWaitTime to avoid the off by one error
                CurrentAmmo--;

                //Run out of ammo - will force reload
                if (CurrentAmmo <= 0)
                {
                    break;
                }
            }

            coolDownCoroutine = StartCoroutine(CoolDown());

            return coolDownCoroutine;
        }


        private IEnumerator Charge()
        {
            //state is charge
            yield return new WaitForSeconds(pfData.ChargeTime.chargeTime);

            Fire();
            //state is cool down
        }

        /// <summary>
        /// Prevents the PF for firing. Used to control rate of fire and forced reloading
        /// </summary>
        /// <param name="waitTime"></param>
        /// <returns></returns>
        private IEnumerator CoolDown()
        {
            State = PFState.CoolDown;
            yield return new WaitForSeconds(pfData.RateOfFire.GetWaitTime(CurrentAmmo));

            //If was a forced reload
            if (CurrentAmmo <= 0)
            {
                CurrentAmmo = pfData.RateOfFire.AmmoCapacity;
            }

            State = PFState.Ready;
        }


        private IEnumerator Reload()
        {
            State = PFState.ManualReload;
            yield return new WaitForSeconds(pfData.RateOfFire.ReloadTime);

            //for now, we are assuming the Overwatch model of ammo - infinte with reloads
            CurrentAmmo = pfData.RateOfFire.AmmoCapacity;
            State = PFState.Ready;
        }
        #endregion

        public override string ToString()
        {
            return string.Format("PF named {0} is in state: {1}, has current ammo {2}", pfData.Meta.name, State.ToString(), CurrentAmmo);
        }
    }
}
