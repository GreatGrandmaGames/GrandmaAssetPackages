using System;
using System.Collections.Generic;
using UnityEngine;

namespace Grandma.Core
{
    [Serializable]
    [CreateAssetMenu(menuName = "Core/Agent Data")]
    public class AgentData : GrandmaComponentData
    {
        public string factionID;
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

        protected override void OnRead(GrandmaComponentData data)
        {
            base.OnRead(data);

            var agentData = Data as AgentData;

            if (agentData != null)
            {
                Faction = GrandmaObjectManager.Instance.GetComponentByID(agentData.factionID, typeof(Faction)) as Faction;
            }
        }

        public void SetFaction(string factionID)
        {
            (Data as AgentData).factionID = factionID;

            Write();
        }
    }
}
