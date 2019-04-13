using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Grandma;
using System;
namespace Grandma
{
    [Serializable]
    [CreateAssetMenu(menuName = "Core/JumpAbility Data")]
    public class JumpAbilityData : AbilityData
    {
        public float jumpForce = 10f;
    }

}
