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
        [Header("Initial Data Options")]
        public GrandmaComponentData initialData;
        public string dataClassName;
        public bool useConventionalDataClass = true;
        [Tooltip("Should this component consider the initial state of the object?")]
        //IE a positionable in the scene will already have meaningful data before runtime
        public bool writeBeforeInitialRead = false;

        //Private Variables
        public GrandmaComponentData Data { get; private set; }

        //Events
        /// <summary>
        /// Called when some data field has been updated
        /// </summary>
        public Action<GrandmaComponent> OnUpdated;

        //Properties
        public GrandmaObject Base { get; private set; }

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

        #region Data Initialisation
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

            if (writeBeforeInitialRead)
            {
                Data = CreateInitialData();

                Refresh();
            }
            else
            {
                Read(CreateInitialData());
            }
        }

        private GrandmaComponentData CreateInitialData()
        {
            if (initialData != null)
            {
                return Instantiate(initialData);
            }

            Debug.Log(dataClassName);

            //Use data class name. If that's null, and we want to use the convention, return convention
            var classNameData = CreateInitialData(dataClassName);
            
            if(classNameData != null)
            {
                return classNameData;
            }
            else if(useConventionalDataClass)
            {
                return CreateInitialData(GetType() + "Data");
            }
            else
            {
                return null;
            }
        }

        private GrandmaComponentData CreateInitialData(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }

            Debug.Log(name);

            var data = ScriptableObject.CreateInstance(name) as GrandmaComponentData;

            if (data != null)
            {
                return data;
            }
            else
            {
                Debug.LogError("GrandmaComponent " + GetType() + " on " + this.name + ": Could not initialise data with name " + name);
                return null;
            }
        }
        #endregion

        protected virtual void Start() { }

        #region Read / Write
        /// <summary>
        /// Set component state from some provided data
        /// </summary>
        /// <param name="data"></param>
        public void Read(GrandmaComponentData data)
        {
            if(data == null)
            {
                Debug.LogError("GrandmaComponent " + GetType() + " on " + name + " has been passed null data");
                return;
            } 

            this.Data = data;

            OnRead(data);

            OnUpdated?.Invoke(this);
        }

        protected void Refresh()
        {
            Write();

            Read(Data);
        }

        /// <summary>
        /// The component should update values based on new Data
        /// </summary>
        /// <param name="data"></param>
        protected virtual void OnRead(GrandmaComponentData data) { }

        /// <summary>
        /// Updates the Data based on the current object state
        /// </summary>
        public void Write()
        {
            if (ValidateState() == false)
            {
                Debug.LogWarning("GrandmaComponent: Cannot Write as Data is invalid");
                return;
            }

            //Ensure ID is set correctly
            Data.associatedObjID = Base.Data.id;

            OnWrite();
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
