﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grandma.Core
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
            //TODO create a data field for movement to include scalar value

            //calculate velocity
            Vector3 newVelocity = (moveHorizontal + moveVertical).normalized * groundMovementData.speedScalar;
            return newVelocity;
        }

        protected override void ApplyVelocity(Vector3 velocity)
        {
            if (velocity.y > 0)
            {
                rb.AddForce(new Vector3(0f, groundMovementData.jumpForce, 0f), ForceMode.Impulse);
            }
            rb.AddForce(velocity, ForceMode.VelocityChange);
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

//NOTE: old code has "movePosition with (rb.position + velocity * Time.deltaTime) 
//if we don't like this ForceMode.VelocityChange movement