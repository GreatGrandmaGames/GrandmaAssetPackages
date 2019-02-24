using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Grandma
{
    [SerializeField]
    [CreateAssetMenu(menuName = "Core/Key Data")]
    public class KeyData : GrandmaComponentData
    {
        public string doorID;
    }
}
