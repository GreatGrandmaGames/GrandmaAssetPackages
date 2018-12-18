using System;
using System.Collections.Generic;
using UnityEngine;

namespace Grandma.Core
{
    public class PositionableData : GrandmaComponentData
    {
        public Vector3 position = Vector3.zero;
        public Vector3 rotation = Vector3.zero;
        public Vector3 localScale = Vector3.one;

        public PositionableData(string id, Transform transform) : base(id)
        {
            SetFromTransform(transform);
        }

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

        protected override void Start()
        {
            base.Start();

            Read(new PositionableData(ObjectID, transform));
        }

        public override void Read(GrandmaComponentData data)
        {
            base.Read(data);

            posData = data as PositionableData;

            if(posData != null)
            {
                transform.position = posData.position;
                transform.rotation = Quaternion.Euler(posData.rotation);
                transform.localScale = posData.localScale;
            }
        }

        protected override void PopulateDataFromInstance()
        {
            base.PopulateDataFromInstance();

            posData.SetFromTransform(transform);
        }
    }
}

