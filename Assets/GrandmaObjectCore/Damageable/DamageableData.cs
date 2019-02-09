using System;
using System.Collections.Generic;
using UnityEngine;

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

}
