using System;
using System.Collections.Generic;
using UnityEngine;

namespace Grandma
{
    public class StunnableData : GrandmaComponentData
    {
        public bool isStunned;
    }

    public class Stunnable : GrandmaComponent
    {
        private Timer timer;
        [NonSerialized]
        private StunnableData stunnableData;

        protected override void Awake()
        {
            base.Awake();

            timer = new Timer();
            timer.OnFinished += Unstun;
        }

        protected override void OnRead(GrandmaComponentData data)
        {
            base.OnRead(data);

            stunnableData = data as StunnableData;
        }

        public void Stun(float time)
        {
            timer.time = time;
            timer.Begin();

            stunnableData.isStunned = true;

            ControlMovement(true);
        }

        public void Unstun()
        {
            if (timer.IsCounting)
            {
                timer.Cancel();
            }

            stunnableData.isStunned = true;

            ControlMovement(false);
        }

        private void ControlMovement(bool active)
        {
            var moveable = this.GetComponent<Moveable>();

            if (moveable)
            {
                if (active)
                {
                    moveable.EnableMovement(false);
                    moveable.ChangeMovementLock.Lock(ComponentID);
                }
                else
                {
                    moveable.ChangeMovementLock.Unlock(ComponentID);
                    moveable.EnableMovement(true);
                }
            }
        }

        /*
        private void ControlContactDamage(bool active)
        {
            var contactDamage = GetComponent<DamageOnContact>();

            if(contactDamage != null)
            {
                
            }
        }
        */
    }
}