using System;
using System.Collections;
using UnityEngine;

using Grandma;

namespace Grandma.ParametricFirearms
{
    public class ParametricFirearm : AgentItem
    {
        [Header("Parametric Firearm Options")]
        public PFProjectile projectilePrefab;
        
        [Tooltip("Where the projectile will spawn from and its initial direction (z-axis)")]
        public Transform barrelTip;

        //Data Properties
        public PFData PFData { get; private set; }

        //Ammo remaining in the clip
        public int CurrentAmmo
        {
            get
            {
                return PFData.Dynamic.CurrentAmmo;
            }
            private set
            {
                PFData.Dynamic.CurrentAmmo = value;

                Write();
            }
        }

        public float CoolDownTimer
        {
            get
            {
                return PFData.Dynamic.CoolDownTime;
            }
            private set
            {
                PFData.Dynamic.CoolDownTime = Mathf.Max(value, 0f);

                Write();
            }
        }

        public float ChargeUpTimer
        {
            get
            {
                return PFData.Dynamic.ChargeUpTime;
            }
            private set
            {
                PFData.Dynamic.ChargeUpTime = Mathf.Min(value, PFData.ChargeTime.chargeTime);

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

            bool isNew = PFData == null;

            PFData = data as PFData;

            if (isNew && PFData != null)
            {
                CurrentAmmo = PFData.RateOfFire.AmmoCapacity;
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
                if(chargeCoroutine != null)
                {
                    StopCoroutine(chargeCoroutine);
                }
                ChargeUpTimer = 0f;

                if (OnTriggerReleased != null)
                {
                    OnTriggerReleased.Invoke(this);
                }

                if (PFData.ChargeTime.requireFullyCharged == false)
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

            for (int i = 0; i < PFData.Multishot.numberOfShots; i++)
            {
                //Spawn the projectile
                var projectile = Instantiate(projectilePrefab);

                if(barrelTip != null)
                {
                    projectile.transform.position = barrelTip.position;
                    projectile.transform.forward = barrelTip.forward;
                }

                //Clone projectile data
                projectile.Launch(this.Agent, this, CreateProjectileData());

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

        private PFProjectileData CreateProjectileData()
        {
            var projData = ScriptableObject.CreateInstance(typeof(PFProjectileData)) as PFProjectileData;

            projData.ImpactDamage = PFData.ImpactDamage;
            projData.AreaDamage = PFData.AreaDamage;
            projData.Trajectory = PFData.Trajectory;

            return projData;
        }

        private IEnumerator Charge()
        {
            ChargeUpTimer = 0f;

            while(ChargeUpTimer < PFData.ChargeTime.chargeTime)
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
            CoolDownTimer = PFData.RateOfFire.GetWaitTime(CurrentAmmo);

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
                CurrentAmmo = PFData.RateOfFire.AmmoCapacity;
            }

            if(OnCoolDownComplete != null)
            {
                OnCoolDownComplete.Invoke(this);
            }

            State = PFState.Ready;
        }

        private IEnumerator ManualReload_CO()
        {
            yield return new WaitForSeconds(PFData.RateOfFire.ReloadTime);

            //for now, we are assuming the Overwatch model of ammo - infinte with reloads
            CurrentAmmo = PFData.RateOfFire.AmmoCapacity;
            State = PFState.Ready;
        }
        #endregion

        public override string ToString()
        { 
            return string.Format("PF named {0} is in state: {1}, has current ammo {2}", PFData.Meta.name, State.ToString(), CurrentAmmo);
        }
    }
}
