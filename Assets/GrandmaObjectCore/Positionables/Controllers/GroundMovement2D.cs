using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grandma
{
    public class GroundMovement2D : RBMove2D
    {
        [System.NonSerialized]
        private GroundMovement2DData groundMovement2DData;

        protected override void OnRead(GrandmaComponentData data)
        {
            base.OnRead(data);
            groundMovement2DData = data as GroundMovement2DData;

        }
        protected override void ApplyVelocity(Vector3 velocity)
        {
            rb.MovePosition(rb.position + (Vector2)velocity * Time.unscaledDeltaTime);
            //rb.AddForce(transform.right * (Vector2) velocity);
        }

        protected override Vector3 CalculateVelocityWithInput(Vector3 InputVector)
        {
            //calculate velocity
            return InputVector * groundMovement2DData.speedScalar;
        }

        protected override Vector3 InputVectorCalculation()
        {
            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");
            //im polling inputting!
            return new Vector3(x, y, 0f);
        }
    }
}

