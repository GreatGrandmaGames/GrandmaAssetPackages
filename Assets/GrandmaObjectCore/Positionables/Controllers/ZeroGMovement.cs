using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace Grandma
{
    //basic zeroG movement for 
    public class ZeroGMovement : RBMove
    {
        [System.NonSerialized]
        private ZeroGMovementData zeroGMovementData;
        protected override void OnRead(GrandmaComponentData data)
        {
            base.OnRead(data);
            zeroGMovementData = data as ZeroGMovementData;

        }

        protected override void ApplyVelocity(Vector3 velocity)
        {
            rb.AddForce(velocity, ForceMode.Acceleration);
        }

        private float originalDrag;
        private float originalAngularDrag;
        public override void Activate()
        {
            base.Activate();
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
            base.Deactivate();
            rb.drag = originalDrag;
            rb.angularDrag = originalAngularDrag;
            rb.useGravity = true;
            rb.velocity = Vector3.zero;
        }

        protected override Vector3 CalculateVelocityWithInput(Vector3 InputVector)
        {
            //regular x movement
            Vector3 moveHorizontal = transform.right * InputVector.x;

            //move in the direction you're facing
            Vector3 moveVertical = Camera.main.transform.forward * InputVector.z;

            //move vertically using the jump button (pharah-esque ??)
            Vector3 moveUp = transform.up * InputVector.y;

            //calculate velocity
            Vector3 newVelocity = (moveHorizontal + moveVertical + moveUp).normalized * zeroGMovementData.thrust;

            return newVelocity;
        }

        protected override Vector3 InputVectorCalculation()
        {
            float x = Input.GetAxisRaw("Horizontal");
            float y = (Input.GetButton("Jump") ? 1 : 0);
            float z = Input.GetAxisRaw("Vertical");
            return new Vector3(x, y, z);
        }
    }
}

