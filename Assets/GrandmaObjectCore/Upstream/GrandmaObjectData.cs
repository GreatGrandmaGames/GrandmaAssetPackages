using System;
using System.Collections.Generic;
using UnityEngine;

namespace Grandma
{
    [Serializable]
    public class GrandmaObjectData 
    {
        public string id;
        public string name;

        public GrandmaObjectData(string id, string name)
        {
            this.id = id;
            this.name = name;
        }

        public bool IsValid()
        {
            return string.IsNullOrEmpty(id) == false && string.IsNullOrEmpty(name);
        }
    }
}
