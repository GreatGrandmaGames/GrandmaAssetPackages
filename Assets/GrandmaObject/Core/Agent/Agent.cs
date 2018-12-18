using System;
using System.Collections.Generic;
using UnityEngine;

namespace Grandma.Core
{
    [Serializable]
    public class AgentData : GrandmaComponentData
    {
        public string factionID;

        public AgentData(string id, string factionID) : base(id)
        {
            this.factionID = factionID;
        }
    }

    public class Agent : GrandmaComponent
    {
        public Faction startingFaction;       

        public Faction Faction { get; private set; }

        public List<AgentItem> Items { get; private set; }

        protected override void Awake()
        {
            base.Awake();

            Items = new List<AgentItem>();
        }

        protected override void Start()
        {
            base.Start();

            if(startingFaction != null)
            {
                Read(new AgentData(ObjectID, startingFaction.ObjectID));
            }
        }

        public override void Read(GrandmaComponentData data)
        {
            base.Read(data);

            var agentData = Data as AgentData;

            if (agentData != null)
            {
                Faction = GrandmaObjectManager.Instance.GetComponentByID(agentData.factionID, typeof(Faction)) as Faction;
            }
        }
    }
}
