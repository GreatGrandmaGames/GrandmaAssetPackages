using System;
using System.Collections;
using UnityEngine;

using Grandma.Core;

namespace Grandma.ParametricFirearms
{
    public class ParametricFirearm : AgentItem
    {
        public PFProjectile projectilePrefab;
        [Tooltip("Where the projectile will spawn from and its initial direction (z-axis)")]
        public Transform barrelTip;

        //Data Properties
        [NonSerialized]
        private PFData pfData;

        //Ammo remaining in the clip
        public int CurrentAmmo
        {
            get
            {
                return pfData.Dynamic.CurrentAmmo;
            }
            private set
            {
                pfData.Dynamic.CurrentAmmo = value;

                Write();
            }
        }

        public float CoolDownTimer
        {
            get
            {
                return pfData.Dynamic.CoolDownTime;
            }
            private set
            {
                pfData.Dynamic.CoolDownTime = Mathf.Max(value, 0f);

                Write();
            }
        }

        public float ChargeUpTimer
        {
            get
            {
                return pfData.Dynamic.ChargeUpTime;
            }
            private set
            {
                pfData.Dynamic.ChargeUpTime = Mathf.Min(value, pfData.ChargeTime.chargeTime);

                Write();
            }
        }

        #region State Management
        public enum PFState
        {
            Ready,
            Charging,
            CoolDown,
            //CoolDownInterupt,
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

                if (OnStateChanged != null)
                {
                    OnStateChanged(value);
                }
            }
        }

        public Action<PFState> OnStateChanged;

        private Coroutine chargeCoroutine;
        private Coroutine manaualReloadCoroutine;
        private Coroutine coolDownCoroutine;
        #endregion

        #region Events
        [Header("Events")]
        public PFEvent OnTriggerPressed;
        public PFPercentageEvent OnCharge;
        public PFEvent OnTriggerReleased;
        public PFEvent OnChargeCancelled;
        public PFEvent OnFire;
        public PFPercentageEvent OnCoolDown;
        public PFEvent OnManualReload;
        public PFEvent OnCancelManualReload;
        public PFEvent OnCoolDownComplete;
        #endregion

        protected override void OnRead(GrandmaComponentData data)
        {
            base.OnRead(data);

            bool isNew = pfData == null;

            pfData = data as PFData;

            if (isNew && pfData != null)
            {
                CurrentAmmo = pfData.RateOfFire.AmmoCapacity;
            }
        }

        #region Public Weapon Methods
        /// <summary>
        /// When in Ready state, will begin charging the weapon. NB if chargeTime is 0, will immediately call fire
        /// </summary>
        public void TriggerPress()
        {
            if (State == PFState.Ready)
            {
                State = PFState.Charging;
                chargeCoroutine = StartCoroutine(Charge());

                if(OnTriggerPressed != null)
                {
                    OnTriggerPressed.Invoke(this);
                }
            }
        }

        /// <summary>
        /// If in Charging state, will either stop charging or fire depending on Data
        /// </summary>
        public void TriggerRelease()
        {
            if (State == PFState.Charging)
            {
                //Interupt charging
                StopCoroutine(chargeCoroutine);
                ChargeUpTimer = 0f;

                if (OnTriggerReleased != null)
                {
                    OnTriggerReleased.Invoke(this);
                }

                if (pfData.ChargeTime.requireFullyCharged == false)
                {
                    //Fire
                    Fire();
                }
                else
                {
                    //Cancel
                    State = PFState.Ready;

                    if (OnChargeCancelled != null)
                    {
                        OnChargeCancelled.Invoke(this);
                    }
                }
            }
        }

        public void ToggleManualReload()
        {
            if(State == PFState.ManualReload)
            {
                CancelManualReload();
            }
            else
            {
                ManualReload();
            }
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

                State = PFState.ManualReload;
                manaualReloadCoroutine = StartCoroutine(ManualReload_CO());

                if (OnManualReload != null)
                {
                    OnManualReload.Invoke(this);
                }
            }
        }

        /// <summary>
        /// If ManualReload, will switch back to ready
        /// </summary>
        public void CancelManualReload()
        {
            if(State == PFState.ManualReload)
            {
                StopCoroutine(manaualReloadCoroutine);
                State = PFState.Ready;

                if (OnManualReload != null)
                {
                    OnManualReload.Invoke(this);
                }
            }
        }

        /*
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
        */
        #endregion

        #region Private Weapon Methods
        /// <summary>
        /// Launches projectile(s) and transistions into cool down
        /// </summary>    
        private void Fire()
        {
            if(projectilePrefab == null)
            {
                Debug.LogWarning("ParametricFirearm: Unable to fire as projectile prefab is null");
                return;
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
                projectile.Launch(this.Agent, this, Instantiate(pfData.Projectile));

                //Controlling ROF
                //CUrrent ammo is decremented before being sent to GetWaitTime to avoid the off by one error
                CurrentAmmo--;

                //Run out of ammo - will force reload
                if (CurrentAmmo <= 0)
                {
                    break;
                }
            }

            if (OnFire != null)
            {
                OnFire.Invoke(this);
            }

            State = PFState.CoolDown;
            coolDownCoroutine = StartCoroutine(CoolDown());
        }

        private IEnumerator Charge()
        {
            ChargeUpTimer = 0f;

            while(ChargeUpTimer < pfData.ChargeTime.chargeTime)
            {
                if(OnCharge != null)
                {
                    OnCharge.Invoke(this, ChargeUpTimer);
                }

                ChargeUpTimer += Time.deltaTime;

                yield return null;
            }

            ChargeUpTimer = 0f;

            //state is charge
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
            CoolDownTimer = pfData.RateOfFire.GetWaitTime(CurrentAmmo);

            while(CoolDownTimer > 0f)
            {
                if (OnCoolDown != null)
                {
                    OnCoolDown.Invoke(this, CoolDownTimer);
                }

                CoolDownTimer -= Time.deltaTime;

                yield return null;
            }

            CoolDownTimer = 0f;

            //If was a forced reload
            if (CurrentAmmo <= 0)
            {
                CurrentAmmo = pfData.RateOfFire.AmmoCapacity;
            }

            if(OnCoolDownComplete != null)
            {
                OnCoolDownComplete.Invoke(this);
            }

            State = PFState.Ready;
        }

        private IEnumerator ManualReload_CO()
        {
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
