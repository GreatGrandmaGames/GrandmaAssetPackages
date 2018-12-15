using UnityEngine;
using System;

namespace Grandma.Core
{
    public class DamageableData : GrandmaComponentData
    {
        public float maxHealth;
        public float currentHealth;

        public DamageableData(string id, float maxHealth) : base(id)
        {
            this.maxHealth = maxHealth;
            this.currentHealth = maxHealth;
        }
    }

    public class Damageable : GrandmaComponent
    {
        [NonSerialized]
        private DamageableData damageData;

        public override void Read(GrandmaComponentData data)
        {
            base.Read(data);

            this.damageData = data as DamageableData;
        }

        public void Damage(float amount)
        {
            if (ValidateState() == false)
            {
                Debug.LogWarning("Damageable: Cannot Heal as Data is not valid");
                return;
            }

            this.damageData.currentHealth -= amount;
        }

        public void Heal(float amount)
        {
            if(ValidateState() == false)
            {
                Debug.LogWarning("Damageable: Cannot Heal as Data is not valid");
                return;
            }

            this.damageData.currentHealth += amount;
        }

        protected override bool ValidateState()
        {
            return base.ValidateState() && damageData != null;
        }
    }
}
