using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grandma
{
    //the basic ground movement for FPS
    public class GroundMovement : RBMove
    {
        [System.NonSerialized]
        private GroundMovementData groundMovementData;
        protected override void OnRead(GrandmaComponentData data)
        {
            base.OnRead(data);
            groundMovementData = data as GroundMovementData;

        }

        protected override Vector3 CalculateVelocityWithInput(Vector3 InputVector)
        {
            Vector3 moveHorizontal = transform.right * InputVector.x;
            Vector3 moveVertical = transform.forward * InputVector.z;

            //calculate velocity
            Vector3 newVelocity = (moveHorizontal + moveVertical).normalized * groundMovementData.speedScalar;


            return newVelocity;
        }

        protected override void ApplyVelocity(Vector3 velocity)
        {
            //rb.AddForce(velocity, ForceMode.VelocityChange);
            rb.MovePosition(rb.position + velocity * Time.deltaTime);

        }

        protected override Vector3 InputVectorCalculation()
        {
            float x = Input.GetAxisRaw("Horizontal");
            float z = Input.GetAxisRaw("Vertical");
            int y = 0;
            return new Vector3(x, y, z);
        }
    }
}