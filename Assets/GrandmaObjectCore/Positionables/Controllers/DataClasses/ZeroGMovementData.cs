using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Grandma
{
    [Serializable]
    [CreateAssetMenu(menuName = "Core/ZeroGMovementData")]
    public class ZeroGMovementData : PositionableData
    {
        [SerializeField]
        public float drag;
        [SerializeField]
        public float angularDrag;
        [SerializeField]
        public float thrust;
    }
}
