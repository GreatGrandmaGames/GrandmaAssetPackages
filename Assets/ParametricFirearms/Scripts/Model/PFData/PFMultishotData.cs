using System;
using System.Collections.Generic;
using UnityEngine;

namespace Grandma.ParametricFirearms
{
    /// <summary>
    /// Parameters to define the trajectory of a projectile
    /// </summary>
    [Serializable]
    public class PFMultishotData : IGrandmaModifiable
    {
        [Tooltip("The number of shots created on launch")]
        public int numberOfShots = 1;
    }
}

