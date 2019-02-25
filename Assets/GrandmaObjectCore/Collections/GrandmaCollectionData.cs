using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Grandma
{
    [Serializable]
    public class GrandmaAssociationData
    {
        public string OtherComponentID;

        public GrandmaAssociationData(string otherCompID)
        {
            this.OtherComponentID = otherCompID;
        }
    }

    [Serializable]
    public class GrandmaCollectionData : GrandmaComponentData
    {
        public bool singleObjectList = true;

        [SerializeField]
        //[HideInInspector]
        public List<GrandmaAssociationData> AssociationData = new List<GrandmaAssociationData>();
  
        public IEnumerable<string> LinkedComponentIDs
        {
            get
            {
                return AssociationData?.Select(x => x.OtherComponentID);
            }
        }
    }
}

