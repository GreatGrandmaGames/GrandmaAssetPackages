using System;
using UnityEngine;

namespace Grandma
{
    [Serializable]
    [CreateAssetMenu(menuName = "Core/Positionable")]
    public class PositionableData : GrandmaComponentData
    {
        [SerializeField]
        public Vector3 position = Vector3.zero;
        [SerializeField]
        public Vector3 rotation = Vector3.zero;
        [SerializeField]
        public Vector3 localScale = Vector3.one;

        public void SetFromTransform(Transform transform)
        {
            position = transform.position;
            rotation = transform.rotation.eulerAngles;
            localScale = transform.localScale;
        }
    }
}
