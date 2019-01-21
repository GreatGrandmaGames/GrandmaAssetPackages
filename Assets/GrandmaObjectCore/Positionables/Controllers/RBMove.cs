using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grandma
{
    [RequireComponent(typeof(Rigidbody))]
    public abstract class RBMove : MoveByVector
    {
        protected Rigidbody rb;
        protected override void Awake()
        {
            base.Awake();
            rb = GetComponent<Rigidbody>();
        }
    }
}
