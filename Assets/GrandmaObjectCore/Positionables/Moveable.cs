using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grandma.Core
{
   
    public abstract class Moveable : Positionable
    {
        public abstract void Move(Vector3 velocity);
        public virtual void Jump(float jumpForce) { }
        //activate specific movement settings
        public virtual void Activate () { }
        //reset them, be a good neighbor
        public virtual void Deactivate() { }

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