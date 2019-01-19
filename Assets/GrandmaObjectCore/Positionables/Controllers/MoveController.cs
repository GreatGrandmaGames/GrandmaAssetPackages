using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grandma.Core
{
    public abstract class MoveController : GrandmaComponent
    {
        protected abstract void ApplyVelocity(Vector3 velocity);
        protected abstract Vector3 CalculateVelocityWithInput(Vector3 InputVector);
        protected abstract Vector3 InputVectorCalculation();

        public virtual void Jump() { }
        //activate specific movement settings
        public virtual void Activate () { }
        //reset them, be a good neighbor
        public virtual void Deactivate() { }

        public void Move()
        {
            Vector3 inputVector = InputVectorCalculation();
            Vector3 velocity = CalculateVelocityWithInput(inputVector);
            ApplyVelocity(velocity);
        }

    }
}
