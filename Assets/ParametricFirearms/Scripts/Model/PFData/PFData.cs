using System;
using UnityEngine;

using Grandma;

namespace Grandma.ParametricFirearms
{
    /// <summary>
    /// All data that defines a PF
    /// </summary>
    [Serializable]
    [CreateAssetMenu(menuName = "ParametricFirearms/Firearm Data")]
    public class PFData : AgentItemData
    {
        [SerializeField]
        public PFMetaData Meta;
        [HideInInspector]
        [SerializeField]
        public PFDynamicData Dynamic;
        [SerializeField]
        public PFImpactDamageData ImpactDamage;
        [SerializeField]
        public PFAreaDamageData AreaDamage;
        [SerializeField]
        public PFTrajectoryData Trajectory;
        [SerializeField]
        public PFRateOfFireData RateOfFire;
        [SerializeField]
        public PFMultishotData Multishot;
        [SerializeField]
        public PFChargeTimeData ChargeTime;

        void Awake()
        {
            this.Meta = this.Meta ?? new PFMetaData();
            this.Dynamic = this.Dynamic ?? new PFDynamicData();
            this.ImpactDamage = this.ImpactDamage ?? new PFImpactDamageData();
            this.AreaDamage = this.AreaDamage ?? new PFAreaDamageData();
            this.Trajectory = this.Trajectory ?? new PFTrajectoryData();
            this.RateOfFire = this.RateOfFire ?? new PFRateOfFireData();
            this.Multishot = this.Multishot ?? new PFMultishotData();
            this.ChargeTime = this.ChargeTime ?? new PFChargeTimeData();
        }
    }

    /// <summary>
    /// Data for the PF that changes rapidly
    /// </summary>
    [Serializable]
    public class PFDynamicData
    {
        public int CurrentAmmo = 0;
        public float CoolDownTime = 0f;
        public float ChargeUpTime = 0f;
    }

    /// <summary>
    /// PF Meta Data
    /// </summary>
    [Serializable]
    public class PFMetaData
    {
        public string name;

        public PFMetaData()
        {
            name = PFRandomNames.GenerateName();
        }
    }
}