using System;
using System.Collections.Generic;
using UnityEngine;

namespace Grandma.Core
{ 
    public class GrandmaObject : MonoBehaviour
    {
        [SerializeField]
        public GrandmaData data;

        public bool IsValid
        {
            get
            {
                return data != null && data.IsValid();
            }
        }

        #region Registration
        private void Awake()
        {
            RegisterWithManager();
        }

        private bool RegisterWithManager()
        {
            if (GrandmaObjectManager.Instance == null)
            {
                //Debug.LogWarning("GrandmaObject: Attempting to create a GrandmaObject before the Manager is created, this is not allowed");             
                GameObject grandmaManager = new GameObject();

                grandmaManager.AddComponent<GrandmaObjectManager>();

                grandmaManager.name = "GrandmaObjectManager";

                //GrandmaObjectManager will set instance on Awake
            }

            GrandmaObjectManager.Instance.Register(this);

            return GrandmaObjectManager.Instance != null;
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

            string fileString = JsonUtility.ToJson(this.data);

            foreach(GrandmaComponent gableObj in GetComponents<GrandmaComponent>())
            {
                header.subData.Add(gableObj.GetType().ToString());

                fileString += "\n";
                fileString += gableObj.WriteToJSON();
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

            var header = JsonUtility.FromJson<GrandmaHeader>(jsonComps[0]);
            var grandmaData = JsonUtility.FromJson<GrandmaData>(jsonComps[1]);

            for(int i = 0; i < header.subData.Count; i++)//each(string s in header.subData ?? new List<string>())
            {
                ComponentRead(jsonComps[i + 2], header.subData[i]);
            }

            this.data = grandmaData;
        } 
        
        //Helper for Read
        private void ComponentRead(string json, string typeString)
        {
            //Extract data from string
            var tempData = JsonUtility.FromJson<GrandmaComponentData>(json);

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
    }
}
