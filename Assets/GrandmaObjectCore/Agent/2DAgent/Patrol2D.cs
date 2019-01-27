using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grandma
{
    public class Patrol2D : RBMove2D
    {
        public float speed;

        public float leftPos;
        public float rightPos;

        private enum Dir
        {
            Left,
            Right
        }
        private Dir currDir;

        protected override void Awake()
        {
            base.Awake();
            currDir = Random.value > 0.5 ? Dir.Left : Dir.Right;
        }

        protected override void ApplyVelocity(Vector3 velocity)
        {
            Debug.Log(rb);
            rb.MovePosition(rb.position + (Vector2)velocity);
        }

        protected override Vector3 CalculateVelocityWithInput(Vector3 InputVector)
        {
            return InputVector.normalized * speed * Time.deltaTime;
        }

        protected override Vector3 InputVectorCalculation()
        {
            Vector3 move = new Vector3();

            if(transform.position.x < leftPos)
            {
                currDir = Dir.Right;
            }
            else if (transform.position.x > rightPos)
            {
                currDir = Dir.Left;
            }

            if (currDir == Dir.Right)
            {
                move.x = 1;
            } else if(currDir == Dir.Left)
            {
                move.x = -1;
            }

            return move;
        }
    }
}
