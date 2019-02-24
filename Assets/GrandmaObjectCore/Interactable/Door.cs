using System;
using System.Collections.Generic;
using UnityEngine;

namespace Grandma
{
    public class Door : GrandmaComponent
    {
        [NonSerialized]
        private DoorData doorData;

        public bool Locked
        {
            get
            {
                return doorData?.locked == true;
            } 
        }

        protected override void OnRead(GrandmaComponentData data)
        {
            base.OnRead(data);

            doorData = data as DoorData;
        }

        public void SetLocked(bool locked)
        {
            doorData.locked = locked;

            gameObject.SetActive(locked);
        }
    }
}
