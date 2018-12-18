using System;

namespace Grandma.Core
{
    [Serializable]
    public class FactionData : GrandmaComponentData
    {
        //all agents

        public FactionData(string id) : base(id)
        {

        }
    }

    public class Faction : GrandmaComponent
    {

    }
}
