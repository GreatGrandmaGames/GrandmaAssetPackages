using System;
using System.Collections.Generic;
using UnityEngine;

namespace Grandma
{
    [Serializable]
    [CreateAssetMenu(menuName = "Core/DamageOnContactData")]
    public class DamageOnContactData : GrandmaComponentData
    {
        public float amount;
    }

    public class DamageOnContact : GrandmaComponent
    {
        private DamageOnContactData docData;

        protected override void OnRead(GrandmaComponentData data)
        {
            base.OnRead(data);
            docData = data as DamageOnContactData;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            collision.gameObject?.GetComponent<Damageable>()?.Damage(new DamageablePayload()
            {
                agentID = ObjectID,
                amount = docData.amount
            });
        }
    }
}
