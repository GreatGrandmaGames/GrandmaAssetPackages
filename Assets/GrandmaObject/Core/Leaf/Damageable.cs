using UnityEngine;
using System;

namespace Grandma.Core
{
    [Serializable]
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

    [Serializable]
    public struct DamageablePayload
    {
        //The modifying object - eg projectile
        public string sourceID;
        //The weapon that fired the damage source
        public string weaponID;
        //The agent that used the weapon
        public string agentID;

        public float amount;

        public DamageablePayload(string agentID, string weaponID, string sourceID, float amount)
        {
            this.sourceID = sourceID;
            this.weaponID = weaponID;
            this.agentID = agentID;
            this.amount = amount;
        }
    }

    public class Damageable : GrandmaComponent
    {
        [NonSerialized]
        private DamageableData damageData;

        public DamageableData startingData;

        protected override void Awake()
        {
            base.Awake();

            Read(startingData);
        }

        public override void Read(GrandmaComponentData data)
        {
            base.Read(data);

            this.damageData = data as DamageableData;
        }

        public void Damage(DamageablePayload payload)
        {
            if (ValidateState() == false)
            {
                Debug.LogWarning("Damageable: Cannot Heal as Data is not valid");
                return;
            }

            this.damageData.currentHealth -= payload.amount;

            UpdatedData();
        }

        public void Heal(DamageablePayload payload)
        {
            if(ValidateState() == false)
            {
                Debug.LogWarning("Damageable: Cannot Heal as Data is not valid");
                return;
            }

            this.damageData.currentHealth += payload.amount;

            UpdatedData();
        }

        protected override bool ValidateState()
        {
            return base.ValidateState() && damageData != null;
        }
    }
}
