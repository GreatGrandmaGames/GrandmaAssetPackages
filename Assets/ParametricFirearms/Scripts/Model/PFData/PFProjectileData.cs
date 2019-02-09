using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grandma.ParametricFirearms
{
    /// <summary>
    /// All data that defines a projectile, as fired by a PF
    /// </summary>
    [Serializable]
    [CreateAssetMenu(menuName = "ParametricFirearms/Projectile Data")]
    public class PFProjectileData : GrandmaComponentData, IGrandmaModifiable
    {
        [SerializeField]
        public PFImpactDamageData ImpactDamage;
        [SerializeField]
        public PFAreaDamageData AreaDamage;
        [SerializeField]
        public PFTrajectoryData Trajectory;
    }
}