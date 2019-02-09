using UnityEngine;
using System;

namespace Grandma
{
    public class Damageable : GrandmaComponent
    {
        [NonSerialized]
        private DamageableData damageData;

        public Action OnDestroyed;

        protected override void OnRead(GrandmaComponentData data)
        {
            base.OnRead(data);

            this.damageData = data as DamageableData;
        }

        public void Damage(DamageablePayload payload)
        {
            if (ValidateState() == false)
            {
                Debug.LogWarning("Damageable: Cannot Damage as Data is not valid");
                return;
            }
            this.damageData.currentHealth -= payload.amount;
            //TODO add flag to DamageableData
            if (this.damageData.currentHealth <= 0)
            {
                OnDestroyed?.Invoke();
            }

            Write();
        }

        public void Heal(DamageablePayload payload)
        {
            if(ValidateState() == false)
            {
                Debug.LogWarning("Damageable: Cannot Heal as Data is not valid");
                return;
            }

            this.damageData.currentHealth += payload.amount;

            Write();
        }

        protected override bool ValidateState()
        {
            return base.ValidateState() && damageData != null;
        }
    }
}
