using System;
using System.Collections.Generic;
using UnityEngine;

namespace Grandma.Core
{
    [Serializable]
    public class GrandmaData
    {
        public string id;
        public string name;

        public bool IsValid()
        {
            return string.IsNullOrEmpty(id) == false && string.IsNullOrEmpty(name);
        }
    }
}
