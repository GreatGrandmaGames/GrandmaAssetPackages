using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Grandma
{
    [Serializable]
    [CreateAssetMenu(menuName = "Core/DoorData")]
    public class DoorData : GrandmaComponentData
    {
        public bool locked;
    }

}
