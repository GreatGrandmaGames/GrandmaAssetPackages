using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Grandma
{
    public abstract class MoveToVector : MoveController
    {
        protected Vector3 target;
        public Vector3 Target
        {
            get 
            {
                return target;
            }
            set
            {
                target = value;

                OnTargetSet(target);
            }
        }

        protected abstract void OnTargetSet(Vector3 target);
    }
}
