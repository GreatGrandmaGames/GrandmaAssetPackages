using System;
using System.Collections.Generic;
using UnityEngine;

namespace Grandma.Core
{
    [Serializable]
    public class GrandmaComponentData
    {
        [HideInInspector]
        public string associatedObjID;
        [HideInInspector]
        public string dataClassName;

        public bool IsValid
        {
            get
            {
                return string.IsNullOrEmpty(associatedObjID) == false && Type.GetType(dataClassName).IsSubclassOf(typeof(GrandmaComponentData));
            }
        }

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
        public GrandmaObject Base { get; private set; }

        private GrandmaComponentData data;
        public GrandmaComponentData Data
        {
            get
            {
                return data;
            }
            set
            {
                data = value;

                if (OnDataRead != null)
                {
                    OnDataRead.Invoke(this);
                }
            }
        }

        /// <summary>
        /// Exposes Read event 
        /// </summary>
        public Action<GrandmaComponent> OnDataRead;
        /// <summary>
        /// Called when some data field has been updated
        /// </summary>
        public Action<GrandmaComponent> OnDataUpdated;

        public string ObjectID
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

        protected virtual void Start() { }

        /// <summary>
        /// Set component state from some provided data
        /// </summary>
        /// <param name="data"></param>
        public virtual void Read(GrandmaComponentData data)
        {
            this.Data = data;
        }

        /// <summary>
        /// Produce a JSON representation of this component
        /// </summary>
        /// <returns></returns>
        public string WriteToJSON()
        {
            if (ValidateState() == false)
            {
                Debug.LogWarning("GrandmaComponent: Cannot Write as Data is invalid");
                return null;
            }

            //Give the component an opportunity to reach out and update any fields it needs to before write
            PopulateDataFromInstance();

            return JsonUtility.ToJson(this.Data);
        }

        protected void UpdatedData()
        {
            if(OnDataUpdated != null)
            {
                OnDataUpdated.Invoke(this);
            }
        }

        /// <summary>
        /// Gives the component a chance to repopulate variables before a Write
        /// </summary>
        protected virtual void PopulateDataFromInstance() { }

        /// <summary>
        /// Is this GrandmaComponent valid?
        /// </summary>
        /// <returns></returns>
        protected virtual bool ValidateState()
        {
            return Data != null && Data.IsValid;
        }
    }
}
