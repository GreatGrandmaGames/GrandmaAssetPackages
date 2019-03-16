using System;
using UnityEngine;

namespace Grandma
{
    [RequireComponent(typeof(GrandmaObject))]
    public abstract class GrandmaComponent : MonoBehaviour
    {
        public enum InitialDataMode
        {
            Convention,
            Provide,
            Named
        }

        [HideInInspector]
        public InitialDataMode initialDataMode;

        //Provide
        [HideInInspector]
        public GrandmaComponentData initialData;

        //Named
        [HideInInspector]
        [Tooltip("The name of the data class to instance")]
        public string dataClassName;
        [HideInInspector]
        [Tooltip("Should the dataClassName include this class' namespace?")]
        public bool appendNameSpace = true;

        [Tooltip("Should this component run its data initialisation straight away? (If this is created in the inspector, then yes!)")]
        public bool initialiseOnAwake = false;

        [Tooltip("Should this component consider the initial state of the object?")]
        //IE a positionable in the scene will already have meaningful data before runtime
        public bool writeBeforeInitialRead = false;

        private bool performedInitialisation = false;

        //Private Variables
        public virtual GrandmaComponentData Data { get; protected set; }

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
            //If spawned in the inspector and no other manager will call its initialisation functions
            if (initialiseOnAwake)
            {
                Init();
            }
        }

        protected virtual void Start() { }

        /// <summary>
        /// Registers this component with the GrandmaObject and, if necessary, the Manager. Always happens on Awake
        /// </summary>
        private void ObjectRegistration()
        {
            //Registration
            Base = GetComponent<GrandmaObject>();

            if (Base == null)
            {
                Base = gameObject.AddComponent<GrandmaObject>();
            }

            if (Base.Data == null)
            {
                Base.RegisterWithManager();
            }
        }

        public void Init()
        {
            ObjectRegistration();
            DataInitialisation();
            InitialiseNewComponent();
        }

        /// <summary>
        /// Creates some initial data for the Component. This should take place before the object is considered valid.
        /// </summary>
        // A flag can be set to call it in Awake, or it can be called manually (i.e. if you want to set variables before
        // creation and delay data initialisation, this is possible.
        private void DataInitialisation()
        {
            if(Data != null)
            {
                Debug.LogWarning("GrandmaComponent: Data Initialisation has already been called on " + GetType());
                return;
            }

            //Initial data
            switch (initialDataMode)
            {
                case InitialDataMode.Provide:
                    Data = Instantiate(initialData);
                    break;
                case InitialDataMode.Named:
                    Data = CreateFromSuppliedString();
                    break;
                case InitialDataMode.Convention:
                    Data = CreateInitialData(GetType() + "Data");
                    break;
            }

            if (Data == null)
            {
                Debug.LogError("GrandmaComponent: Initial Data Creation has failed. Removing this component.");
                return;
            }

            //Initialise
            if (writeBeforeInitialRead)
            {
                Write();
            }

            Read(Data);
        }

        /// <summary>
        /// Create any related objects. Should only performed when / where the object is created
        /// </summary>
        //E.g. create any associated connections
        private void InitialiseNewComponent()
        {
            //This method can only be called once
            if (performedInitialisation == false)
            {
                performedInitialisation = true;

                OnCreated();

                Refresh();
            } 

            //else "GrandmaComponent: Component has already been initialised");
        }

        protected virtual void OnCreated() { }

        //Helper for DataInitialisation
        private GrandmaComponentData CreateFromSuppliedString()
        {
            if (string.IsNullOrEmpty(dataClassName))
            {
                return null;
            }

            string n = dataClassName;

            if (appendNameSpace)
            {
                n.Insert(0, GetType().Namespace + ".");
            }

            return CreateInitialData(n);
        }

        //Helper for DataInitialisation
        private GrandmaComponentData CreateInitialData(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }

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

        public void Refresh()
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
