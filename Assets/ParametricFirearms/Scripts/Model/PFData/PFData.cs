using System;
using UnityEngine;

using Grandma.Core;

namespace Grandma.ParametricFirearms
{
    /// <summary>
    /// All data that defines a PF
    /// </summary>
    [CreateAssetMenu(menuName = "ParametricFirearms/Firearm Data")]
    public class PFData : AgentItemData
    {
        [SerializeField]
        public PFMetaData Meta;
        [HideInInspector]
        [SerializeField]
        public PFDynamicData Dynamic;
        [SerializeField]
        public PFProjectileData Projectile;
        [SerializeField]
        public PFRateOfFireData RateOfFire;
        [SerializeField]
        public PFMultishotData Multishot;
        [SerializeField]
        public PFChargeTimeData ChargeTime;

        void Awake()
        {
            this.Meta = new PFMetaData();
            this.Dynamic = new PFDynamicData();
            if(this.Projectile == null)
            {
                this.Projectile = CreateInstance<PFProjectileData>();
            }
            this.RateOfFire = new PFRateOfFireData();
            this.Multishot = new PFMultishotData();
            this.ChargeTime = new PFChargeTimeData();
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