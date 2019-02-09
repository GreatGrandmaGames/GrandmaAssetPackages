using System;
using System.Collections.Generic;
using UnityEngine;

namespace Grandma
{
    public class Positionable : GrandmaComponent
    {
        [NonSerialized]
        private PositionableData posData;

        public override GrandmaComponentData Data { get => base.Data;

            protected set
            {
                base.Data = value;

                posData = value as PositionableData;
            }
        }

        protected override void OnRead(GrandmaComponentData data)
        {
            base.OnRead(data);

            if(posData != null)
            {
                transform.position = posData.position;
                transform.rotation = Quaternion.Euler(posData.rotation);
                transform.localScale = posData.localScale;
            }
        }

        protected override void OnWrite()
        {
            base.OnWrite();

            posData.SetFromTransform(transform);
        }
    }
}

