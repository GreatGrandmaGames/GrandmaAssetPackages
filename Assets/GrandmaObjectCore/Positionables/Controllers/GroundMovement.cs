using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grandma
{
    [CreateAssetMenu(menuName = "Core/GroundMovementData")]
    public class GroundMovementData : PositionableData
    {
        public float speedScalar;
        public float jumpForce;
    }

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

            if (InputVector.y > 0)
            {
                newVelocity.y = InputVector.y;
            }
            return newVelocity;
        }

        protected override void ApplyVelocity(Vector3 velocity)
        {
            if (velocity.y > 0)
            {
                rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
                rb.AddForce(new Vector3(0f, groundMovementData.jumpForce, 0f), ForceMode.Impulse);
            }
            //rb.AddForce(velocity, ForceMode.VelocityChange);
            rb.MovePosition(rb.position + velocity * Time.unscaledDeltaTime);

        }

        protected override Vector3 InputVectorCalculation()
        {
            float x = Input.GetAxisRaw("Horizontal");
            float z = Input.GetAxisRaw("Vertical");
            int y = (Input.GetButtonDown("Jump") ? 1 : 0);
            return new Vector3(x, y, z);
        }
    }
}