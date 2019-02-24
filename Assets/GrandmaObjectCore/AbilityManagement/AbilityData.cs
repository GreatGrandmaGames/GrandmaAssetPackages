using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Grandma;
using System;
namespace Grandma
{
    [Serializable]
    [CreateAssetMenu(menuName = "Core/Ability Data")]
    public class AbilityData : GrandmaComponentData
    {
        public float coolDownTime;
    }

}
