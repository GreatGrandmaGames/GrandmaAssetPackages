using System;
using UnityEngine;

namespace Grandma.ParametricFirearms
{
    /// <summary>
    /// Parameters to define the aread damage of a projectile
    /// </summary>
    [Serializable]
    public class PFAreaDamageData : IGrandmaModifiable
    {
        [Tooltip("Does this projectile explode")]
        [SerializeField]
        public bool explodable;

        [Tooltip("The damage caused if distance to impact is zero")]
        [SerializeField]
        public float maxDamage;
        [Tooltip("The distance at which damage falls to zero. mEasured in meters")]
        [SerializeField]
        public float maxBlastRange;

        //1 = rocket like behaviour - explodes on 1st impact
        [Tooltip("The number of collisions this projectile can withstand before exploding")]
        [SerializeField]
        public int numImpactsToDetonate;
    }
}
