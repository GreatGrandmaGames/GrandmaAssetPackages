using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace Grandma
{
    [Serializable]
    public class AgentItemData : GrandmaComponentData
    {
        [HideInInspector]
        public string agentID;
    }

}