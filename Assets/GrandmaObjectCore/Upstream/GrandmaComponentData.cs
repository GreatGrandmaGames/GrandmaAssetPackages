using System;
using System.Collections.Generic;
using UnityEngine;

namespace Grandma.Core
{
    public class GrandmaComponentData : ScriptableObject
    {
        [HideInInspector]
        public string associatedObjID;
        [HideInInspector]
        public string dataClassName;

        private void Awake()
        {
            this.dataClassName = this.GetType().ToString();
        }

        public virtual bool IsValid
        {
            get
            {

                //Associated object ID only needs to be set on a write
                return /*string.IsNullOrEmpty(associatedObjID) == false && */Type.GetType(dataClassName).IsSubclassOf(typeof(GrandmaComponentData));
            }
        }
    }
}

