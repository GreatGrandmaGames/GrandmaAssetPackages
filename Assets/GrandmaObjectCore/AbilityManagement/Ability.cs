using System;
using System.Collections.Generic;
using UnityEngine;

namespace Grandma
{
    public abstract class Ability : GrandmaComponent
    {
        public string enteringKey;

        public Timer CoolDown { get; private set; } = new Timer();

        protected override void OnRead(GrandmaComponentData data)
        {
            base.OnRead(data);

            var abil = Data as AbilityData;

            if(abil != null)
            {
                CoolDown.time = abil.coolDownTime;
            }
        }

        public virtual bool CanEnter()
        {
            return CoolDown.IsCounting == false;
        }

        /// <summary>
        /// Once entered, this will determine when the ability actives
        /// </summary>
        public virtual bool WillActivate()
        {
            return true;
        }

        public virtual void Enter() { }

        public abstract void Activate();

        public virtual void Exit() 
        {
            CoolDown.Begin();
        }
    }
}
