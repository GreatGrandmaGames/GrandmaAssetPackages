using System;
using UnityEngine.Events;

namespace Grandma.ParametricFirearms
{
    [Serializable]
    public class PFEvent : UnityEvent<ParametricFirearm> { }
    [Serializable]
    public class PFPercentageEvent : UnityEvent<ParametricFirearm, float> { }
}
