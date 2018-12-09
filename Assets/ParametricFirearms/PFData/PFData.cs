using System;
using UnityEngine;

using Grandma.Core;

namespace Grandma.ParametricFirearms
{
    /// <summary>
    /// All data that defines a PF
    /// </summary>
    [Serializable]
    public class PFData : GrandmaComponentData
    {
        [SerializeField]
        public PFMetaData Meta;
        [SerializeField]
        public PFProjectileData Projectile;
        [SerializeField]
        public PFRateOfFireData RateOfFire;
        [SerializeField]
        public PFMultishotData Multishot;
        [SerializeField]
        public PFChargeTimeData ChargeTime;

        public PFData(string id) : base(id)
        {
            this.Meta = new PFMetaData();
            this.Projectile = new PFProjectileData(id);
            this.RateOfFire = new PFRateOfFireData();
            this.Multishot = new PFMultishotData();
            this.ChargeTime = new PFChargeTimeData();
        }
    }

    /// <summary>
    /// All data that defines a projectile, as fired by a PF
    /// </summary>
    [Serializable]
    public class PFProjectileData : GrandmaComponentData
    {
        [SerializeField]
        public PFImpactDamageData ImpactDamage;
        [SerializeField]
        public PFAreaDamageData AreaDamage;
        [SerializeField]
        public PFTrajectoryData Trajectory;

        public PFProjectileData(string id) : base(id)
        {
            this.ImpactDamage = new PFImpactDamageData();
            this.AreaDamage = new PFAreaDamageData();
            this.Trajectory = new PFTrajectoryData();
        }
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