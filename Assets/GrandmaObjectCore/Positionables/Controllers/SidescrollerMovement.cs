using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grandma
{
    [CreateAssetMenu(menuName = "Core/MovementData/2D/SidescrollerMovementData")]
    public class SidescrollerMovementData : GrandmaComponentData
    {
        public float speedScalar = 3f;
        public float jumpForce = 10f;

        [HideInInspector]
        public bool canJump = true;

        public float maxSpeed = 5f;
        public int numberOfJumps = 2;
    }

    public class SidescrollerMovement : RBMove2D
    {
        [System.NonSerialized]
        private SidescrollerMovementData sidescrollerMovementData;
        private bool jump = false;

        private bool hasInitialized = false;
        private int currentJumps = 0;

        protected override void Awake()
        {
            base.Awake();
            base.Activate();
        }

        protected override void OnRead(GrandmaComponentData data)
        {
            base.OnRead(data);
            sidescrollerMovementData = data as SidescrollerMovementData;

            if (hasInitialized == false)
            {
                hasInitialized = true;
                currentJumps = sidescrollerMovementData.numberOfJumps;
            }

        }
        protected override void ApplyVelocity(Vector3 velocity)
        {
            if (Mathf.Sign(velocity.x) * rb.velocity.x < sidescrollerMovementData.maxSpeed)
            {
                rb.AddForce(velocity);
            }

            if (Mathf.Abs(rb.velocity.x) > sidescrollerMovementData.maxSpeed)
            {
                rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * sidescrollerMovementData.maxSpeed, rb.velocity.y);
            }

            //jump
            if (jump)
            {
                currentJumps--;
                rb.velocity = new Vector2(rb.velocity.x, 0f);
                rb.AddForce(new Vector2(0f, sidescrollerMovementData.jumpForce));

                if (currentJumps <= 0)
                {
                    sidescrollerMovementData.canJump = false;
                }
                jump = false;
            }
        }

        protected override Vector3 CalculateVelocityWithInput(Vector3 InputVector)
        {
            //calculate velocity
            return new Vector3(InputVector.x * sidescrollerMovementData.speedScalar, 0f, 0f);
        }
        void Update()
        {
            if (Input.GetButtonDown("Jump") && sidescrollerMovementData.canJump)
            {
                jump = true;
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                sidescrollerMovementData.canJump = true;
                currentJumps = sidescrollerMovementData.numberOfJumps;
            }
        }

        protected override Vector3 InputVectorCalculation()
        {
            float x = Input.GetAxisRaw("Horizontal");
            //im polling inputting in FixedUpdate!
            return new Vector3(x, 0f, 0f);
        }
    }
}
