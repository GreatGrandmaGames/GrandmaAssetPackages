using System;
using UnityEngine;

namespace Grandma.ParametricFirearms
{
    /// <summary>
    /// Parameters to define the charge-up properties of a PF
    /// </summary>
    [Serializable]
    public class PFChargeTimeData : IGrandmaModifiable
    {
        [Tooltip("The time to fully charge the weapon. Measured in seconds")]
        public float chargeTime;
        [Tooltip("Does the weapon need to be fully charged in odrer to fire?")]
        public bool requireFullyCharged;
    }
}

