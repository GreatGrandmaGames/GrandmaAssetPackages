using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Grandma
{
    [RequireComponent(typeof(GrandmaObject))]
    public abstract class GrandmaComponent : MonoBehaviour
    {
        public GrandmaObject Base { get; private set; }

        [Header("Data Options")]
        [Tooltip("Should this component use the canonical Scriptable Object or create an instance for its own use?")]
        public bool duplicateData = true;

        public GrandmaComponentData Data;

        /// <summary>
        /// Called when some data field has been updated
        /// </summary>
        public Action<GrandmaComponent> OnUpdated;

        public string ObjectID
        {
            get
            {
                return Base?.Data.id;
            }
        }

        public string ComponentID
        {
            get
            {
                return Data.componentID;
            }
        }

        protected virtual void Awake()
        {
            Base = GetComponent<GrandmaObject>();

            if (Base == null)
            {
                Base = gameObject.AddComponent<GrandmaObject>();
            }

            if(Base.Data == null)
            {
                Base.RegisterWithManager();
            }

            if (Data != null)
            {
                if (duplicateData)
                {
                    Data = Data.Clone();

                    //Debug.Log("GComp: Clone " + Data.SerializeJSON());
                }

                Read(Data);
            }
        }

        protected virtual void Start() { }

        #region Read
        /// <summary>
        /// Set component state from some provided data
        /// </summary>
        /// <param name="data"></param>
        public void Read(GrandmaComponentData data)
        {
            if(data == null)
            {
                return;
            } 

            this.Data = data;

            OnRead(data);

            if(OnUpdated != null)
            {
                OnUpdated(this);
            }
        }

        protected virtual void OnRead(GrandmaComponentData data) { }
        #endregion

        #region Write
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
            OnWrite();

            return Data.SerializeJSON();
        }

        //Helper - alert that data has changed
        protected virtual void Write()
        {
            if (ValidateState() == false)
            {
                Debug.LogWarning("GrandmaComponent: Cannot Write as Data is invalid");
                return;
            }

            Data.associatedObjID = Base.Data.id;

            //Update based on new Data
            Read(Data);
        }

        /// <summary>
        /// Gives the component a chance to repopulate variables before a Write
        /// </summary>
        protected virtual void OnWrite() { }
        #endregion

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
