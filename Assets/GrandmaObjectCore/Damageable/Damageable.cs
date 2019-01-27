using UnityEngine;
using System;

namespace Grandma
{
    [Serializable]
    [CreateAssetMenu(menuName = "Core/Damageable Data")]
    public class DamageableData : GrandmaComponentData
    {
        public float maxHealth;
        public float currentHealth;
    }

    [Serializable]
    public struct DamageablePayload
    {
        //The optional modifying object - eg projectile
        public string sourceID;
        //The optional agent that used the weapon
        public string agentID;

        public float amount;

        public DamageablePayload(string agentID, string sourceID, float amount)
        {
            this.sourceID = sourceID;
            this.agentID = agentID;
            this.amount = amount;
        }
    }

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
                LevelManager.Instance.Reload();
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
