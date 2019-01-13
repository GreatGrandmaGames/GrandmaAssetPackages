using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grandma.Core
{
    public class GroundMovement : RBMove
    {
        public override void Move(Vector3 velocity)
        {
            rb.AddForce(velocity, ForceMode.VelocityChange);
        }
    }
}

