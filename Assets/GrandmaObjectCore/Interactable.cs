using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Grandma
{
    /// <summary>
    /// Triggered by collider
    /// </summary>
    public abstract class Interactable : MonoBehaviour
    {
        private Collider2D triggerCol;

        protected abstract void OnTriggered(string triggeringID);

        private void OnCollisionEnter2D(Collision2D collision)
        {
            var agent = collision.collider?.GetComponent<Agent>();

            if (agent != null)
            {
                OnTriggered(agent.ObjectID);
            }
        }
    }
}