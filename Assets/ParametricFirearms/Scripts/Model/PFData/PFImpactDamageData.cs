using System;
using System.Collections.Generic;
using UnityEngine;

namespace Grandma.ParametricFirearms
{
    /// <summary>
    /// Parameters to define the trajectory of a projectile
    /// </summary>
    [Serializable]
    public class PFImpactDamageData : IGrandmaModifiable
    {
        [Tooltip("The base damage caused by an impact of a projectile on a Damageable object. A negative value will heal")]
        [SerializeField]
        public float baseDamage;

        [Tooltip("The rate of damage change over distance travelled. Measured in 1/meters.")]
        [SerializeField]
        public float damageChangeByDistance;
    }
}
