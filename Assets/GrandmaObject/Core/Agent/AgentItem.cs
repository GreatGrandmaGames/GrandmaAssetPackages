using System;
using UnityEngine;

namespace Grandma.Core
{
    [Serializable]
    public class AgentItemData : GrandmaComponentData
    {
        [HideInInspector]
        public string agentID;

        public AgentItemData(string id, string agentID = "") : base(id)
        {
            this.agentID = agentID;
        }
    }

    /// <summary>
    /// An item that can be used by an agent
    /// </summary>
    public abstract class AgentItem : GrandmaComponent
    {
        private Agent agent;
        public Agent Agent
        {
            get
            {
                return agent;
            }
            set
            {
                if(agent != null && agent.Items.Contains(this))
                {
                    agent.Items.Remove(this);
                }

                agent = value;

                if (agent != null && agent.Items.Contains(this) == false)
                {
                    agent.Items.Add(this);
                }
            }
        }

        public Agent startingAgent;

        protected override void Start()
        {
            base.Start();

            if (startingAgent != null)
            {
                Read(new AgentItemData(ObjectID, startingAgent.ObjectID));
            } 
        }

        public override void Read(GrandmaComponentData data)
        {
            base.Read(data);

            var agentItemData = Data as AgentItemData;

            if (agentItemData != null)
            {
                Agent = GrandmaObjectManager.Instance.GetComponentByID(agentItemData.agentID, typeof(Agent)) as Agent;
            }
        }
    }
}
