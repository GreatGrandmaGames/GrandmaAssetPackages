using System;
using System.Collections.Generic;
using UnityEngine;

namespace Grandma
{
    public class Key : Interactable
    {
        public Door startingDoor;
        public bool destroyOnUnlock;

        [NonSerialized]
        private KeyData keyData;

        protected override void OnRead(GrandmaComponentData data)
        {
            base.OnRead(data);

            keyData = data as KeyData;
        }

        protected override void OnTriggered(string triggeringID)
        {
            /*
            var door = GrandmaObjectManager.Instance.GetComponentByObjectID<Door>(keyData?.doorID);
            */
            //door?.Lock(false);
            Debug.Log("Triggering id " + triggeringID);
            
            startingDoor?.SetLocked(false);

            if (destroyOnUnlock)
            {
                Destroy(gameObject);
            }
        }

        public void SetDoor(Door d)
        {
            keyData.doorID = d.ObjectID;
        }
    }
}

