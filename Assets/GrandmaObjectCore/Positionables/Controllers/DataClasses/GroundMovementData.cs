using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Grandma
{
    [CreateAssetMenu(menuName = "Core/GroundMovementData")]
    public class GroundMovementData : PositionableData
    {
        [SerializeField]
        public float speedScalar;
        [SerializeField]
        public float jumpForce;
    }
}
