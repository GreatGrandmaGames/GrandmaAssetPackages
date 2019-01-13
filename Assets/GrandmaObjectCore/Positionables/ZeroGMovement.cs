using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grandma.Core
{
    [CreateAssetMenu(menuName = "Core/ZeroGMovementData")]
    public class ZeroGMovementData: PositionableData
    {
        public float drag;
        public float angularDrag;
    }

    public class ZeroGMovement : RBMove
    {
        [System.NonSerialized]
        private ZeroGMovementData zeroGMovementData;
        protected override void OnRead(GrandmaComponentData data)
        {
            base.OnRead(data);
            zeroGMovementData = data as ZeroGMovementData;

        }

        public override void Move(Vector3 velocity)
        {
           
            rb.AddForce(velocity, ForceMode.Acceleration);
        }

        private float originalDrag;
        private float originalAngularDrag;
        public override void Activate()
        { 
            rb.useGravity = false;
            originalDrag = rb.drag;
            originalAngularDrag = rb.angularDrag;
            if (zeroGMovementData != null)
            {
               
                rb.drag = zeroGMovementData.drag;
                rb.angularDrag = zeroGMovementData.angularDrag;
            }
        }
        public override void Deactivate()
        {
            rb.drag = originalDrag;
            rb.angularDrag = originalAngularDrag;
            rb.useGravity = true;
        }
    }
}

