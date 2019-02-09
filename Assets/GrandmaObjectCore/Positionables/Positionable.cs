using System;
using System.Collections.Generic;
using UnityEngine;

namespace Grandma
{
    [Serializable]
    [CreateAssetMenu(menuName = "Core/Positionable")]
    public class PositionableData : GrandmaComponentData
    {
        public Vector3 position = Vector3.zero;
        public Vector3 rotation = Vector3.zero;
        public Vector3 localScale = Vector3.one;

        public void SetFromTransform(Transform transform)
        {
            position = transform.position;
            rotation = transform.rotation.eulerAngles;
            localScale = transform.localScale;
        }
    }

    public class Positionable : GrandmaComponent
    {
        [NonSerialized]
        private PositionableData posData;
        protected override void OnRead(GrandmaComponentData data)
        {
            base.OnRead(data);

            posData = data as PositionableData;

            if(posData != null)
            {
                //transform.position = posData.position;
                //transform.rotation = Quaternion.Euler(posData.rotation);
                //transform.localScale = posData.localScale;
            }
        }

        protected override void OnWrite()
        {
            base.OnWrite();

            //posData.SetFromTransform(transform);
        }
    }
}

