using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grandma
{
    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class RBMove2D : MoveByVector
    {
        protected Rigidbody2D rb;
        protected override void Awake()
        {
            base.Awake();
            rb = GetComponent<Rigidbody2D>();
        }
    }
}
