using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grandma
{
    public abstract class MoveController : GrandmaComponent
    {
        public virtual bool CanMove { get; set; } = true;

        protected bool active { get; private set; }

        public virtual void Jump() { }
        //activate specific movement settings
        public virtual void Activate () 
        {
            active = true;
        }
        //reset them, be a good neighbor
        public virtual void Deactivate() 
        {
            active = false;
        }
    }
}
