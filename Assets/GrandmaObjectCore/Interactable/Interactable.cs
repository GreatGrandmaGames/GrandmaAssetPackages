using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Grandma
{
    /// <summary>
    /// Triggered by collider
    /// </summary>
    public abstract class Interactable : GrandmaComponent
    {
        private Collider2D triggerCol;

        protected abstract void OnTriggered(string triggeringID);

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var agent = collision?.GetComponentInParent<Agent>();

            if (agent != null)
            {
                OnTriggered(agent.ObjectID);
            }
        }
    }
}