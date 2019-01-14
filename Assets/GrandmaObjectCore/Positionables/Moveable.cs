using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grandma.Core
{
   
    public abstract class Moveable : Positionable
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

    [RequireComponent(typeof(Rigidbody))]
    public abstract class RBMove : Moveable
    {
        protected Rigidbody rb;
        protected override void Awake()
        {
            base.Awake();
            rb = GetComponent<Rigidbody>();
        }
    }
}


/*        //private Rigidbody rb;
        protected override void Awake()
        {
            base.Awake();
            rb = GetComponent<Rigidbody>();
        }
        */