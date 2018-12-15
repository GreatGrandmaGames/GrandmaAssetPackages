using System;
using System.Collections.Generic;
using UnityEngine;

namespace Grandma.Core
{
    [Serializable]
    public class GrandmaComponentData
    {
        public bool isValid = true;
        public string associatedObjID;
        public string dataClassName;

        public GrandmaComponentData(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("ID Cannot Be Null or Empty");
            }

            this.associatedObjID = id;
            this.dataClassName = this.GetType().ToString();
        }
    }

    [RequireComponent(typeof(GrandmaObject))]
    public abstract class GrandmaComponent : MonoBehaviour
    {
        [HideInInspector]
        public GrandmaObject Base;

        public GrandmaComponentData Data { get; private set; }

        public string GrandmaObjectID
        {
            get
            {
                return Base.data.id;
            }
        }

        protected virtual void Awake()
        {
            Base = GetComponent<GrandmaObject>();

            if (Base == null)
            {
                Base = gameObject.AddComponent<GrandmaObject>();
            }
        }

        public virtual void Read(GrandmaComponentData data)
        {
            this.Data = data;
        }

        public string WriteToJSON()
        {
            if (ValidateState() == false)
            {
                Debug.LogWarning("GrandmaComponent: Cannot Write as Data is invalid");
                return null;
            }

            return JsonUtility.ToJson(this.Data);
        }

        protected virtual bool ValidateState()
        {
            return Data != null && Data.isValid;
        }
    }
}
