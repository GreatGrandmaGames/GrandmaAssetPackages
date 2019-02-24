using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Grandma
{ 
    public class GrandmaObject : MonoBehaviour
    {
        [HideInInspector]
        [SerializeField]
        public GrandmaObjectData Data { get; private set; }

        public bool IsValid
        {
            get
            {
                return Data != null && Data.IsValid();
            }
        }

        #region Registration
        private void Awake()
        {
            RegisterWithManager();
        }

        public void RegisterWithManager()
        {
            if(Data != null)
            {
                //already set data
                return;
            }

            if (GrandmaObjectManager.Instance == null)
            {
                GameObject grandmaManager = new GameObject();

                grandmaManager.AddComponent<GrandmaObjectManager>();

                grandmaManager.name = "GrandmaObjectManager";

                //GrandmaObjectManager will set instance on Awake
            }

            Data = GrandmaObjectManager.Instance.Register(this);
        }

        private bool isQutting = false;
        private void OnApplicationQuit()
        {
            isQutting = true;
        }

        private void OnDestroy()
        {
            if(isQutting == false)
            {
                GrandmaObjectManager.Instance.Unregister(this);
            }
        }
        #endregion

        #region Read / Write
        [Serializable]
        private class GrandmaHeader
        {
            [SerializeField]
            public List<string> subData = new List<string>();
        }

        public string WriteJSON()
        {
            if (IsValid == false)
            {
                RegisterWithManager();
            }

            var header = new GrandmaHeader();

            string fileString = JsonUtility.ToJson(this.Data);

            foreach(GrandmaComponent gableObj in GetComponents<GrandmaComponent>())
            {
                header.subData.Add(gableObj.GetType().ToString());

                fileString += "\n";
                fileString += gableObj.Data.SerializeJSON();
            }

            var finalString = JsonUtility.ToJson(header) + "\n" + fileString;

            return finalString;
        }

        public void ReadJSON(string jsonString)
        {
            if(IsValid == false)
            {
                RegisterWithManager();
            }

            if(string.IsNullOrEmpty(jsonString))
            {
                Debug.LogError("GrandmaObject: Cannot read null");
                return;
            }

            var jsonComps = jsonString.Split('\n');

            if(jsonComps.Length < 2)
            {
                Debug.LogError("GrandmaObject: Invalid. Must contain at least meta and GrandmaData");
                return;
            }

            var header = JsonUtility.FromJson(jsonComps[0], typeof(GrandmaHeader)) as GrandmaHeader;
            var grandmaData = JsonUtility.FromJson(jsonComps[1], typeof(GrandmaObjectData)) as GrandmaObjectData;

            for(int i = 0; i < header.subData.Count; i++)//each(string s in header.subData ?? new List<string>())
            {
                ComponentRead(jsonComps[i + 2], header.subData[i]);
            }

            this.Data = grandmaData;
        } 
        
        //Helper for Read
        private void ComponentRead(string json, string typeString)
        {
            //Extract data from string
            var tempData = JsonUtility.FromJson(json, typeof(GrandmaComponentData)) as GrandmaComponentData;

            Type dataType = Type.GetType(tempData.dataClassName);

            GrandmaComponentData data = JsonUtility.FromJson(json, dataType) as GrandmaComponentData;

            //Find component
            Type componentType = Type.GetType(typeString);

            Component component = GetComponent(componentType);

            if (component == null)
            {
                component = gameObject.AddComponent(componentType);
            }

            GrandmaComponent gComp = component as GrandmaComponent;

            //Component read data
            if (gComp != null)
            {
                gComp.Read(data);
            }
        }
        #endregion

        #region Component Retrival
        public GrandmaComponent GetComponentByID(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return null;
            }

            return GetComponents<GrandmaComponent>().FirstOrDefault(x => x.ComponentID == id);
        }
        #endregion
    }
}
