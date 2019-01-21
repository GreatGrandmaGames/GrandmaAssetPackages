using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Grandma
{
    public abstract class MoveByVector : MoveController
    {
        protected abstract void ApplyVelocity(Vector3 velocity);
        protected abstract Vector3 CalculateVelocityWithInput(Vector3 InputVector);
        protected abstract Vector3 InputVectorCalculation();

        void FixedUpdate()
        {
            if (active && CanMove)
            {
                Vector3 inputVector = InputVectorCalculation();
                Vector3 velocity = CalculateVelocityWithInput(inputVector);
                ApplyVelocity(velocity);
            }
        }
    }
}
