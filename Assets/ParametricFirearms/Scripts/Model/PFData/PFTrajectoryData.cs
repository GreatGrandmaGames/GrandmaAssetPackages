using System;
using UnityEngine;

namespace Grandma.ParametricFirearms
{
    /// <summary>
    /// Parameters to define the trajectory of a projectile
    /// </summary>
    [Serializable]
    public class PFTrajectoryData : IGrandmaModifiable
    {
        [Tooltip("The launch force applied to a shot projectile. Measured in Newtons")]
        [SerializeField]
        public float initialForceVector;
        [Tooltip("The maximum deviation applied to a shot projectile at launch time.")]
        [SerializeField]
        public float maxInitialSpreadAngle;
        [Tooltip("The drop off of a projectile as a percentage of its initialForceVector.")]
        [SerializeField]
        public float dropOffRatio; 
    }
}
