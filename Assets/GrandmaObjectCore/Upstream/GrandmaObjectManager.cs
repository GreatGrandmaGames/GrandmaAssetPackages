using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Grandma
{
    public class GrandmaObjectManager : MonoBehaviour
    {
        //Variables
        private List<GrandmaObject> allObjects = new List<GrandmaObject>();

        #region Singleton
        private static GrandmaObjectManager instance;
        public static GrandmaObjectManager Instance
        {
            get
            {
                if(instance == null)
                {
                    var go = new GameObject("GrandmaObjectManager");
                    instance = go.AddComponent<GrandmaObjectManager>();
                }

                return instance;
            }
        }
        #endregion

        #region Registration
        public GrandmaObjectData Register(GrandmaObject gObj)
        {
            if (gObj == null)
            {
                Debug.LogError("GrandmaObjectManager -> Register(): Provided object is null");
                return null;
            }

            allObjects.Add(gObj);

            return new GrandmaObjectData(Guid.NewGuid().ToString(), gObj.name);
        }

        public void Unregister(GrandmaObject gObj)
        {
            if (gObj == null)
            {
                Debug.LogWarning("GrandmaObjectManager -> Unregister(): Provided object is null");
                return;
            }

            if (allObjects.Contains(gObj) == false)
            {
                Debug.LogWarning("GrandmaObjectManager -> Unregister(): Object was not registered");
                return;
            }

            //NB: the IDGenerator will notreuse an unregistered object's ID

            allObjects.Remove(gObj);
        }
        #endregion

        #region Object & Component Retrival
        public List<GrandmaObject> AllObjects
        {
            get
            {
                var cloneList = new List<GrandmaObject>();
                cloneList.AddRange(allObjects);
                return cloneList;
            }
        }
        
        public GrandmaObject GetByObjectID(string id)
        {
            return allObjects.SingleOrDefault(x => x.Data.id == id);
        }

        public T GetComponentByID<T>(string componentID) where T : GrandmaComponent
        {
            return GetComponentByID(componentID) as T;
        }

        public GrandmaComponent GetComponentByID(string componentID)
        {
            foreach(var go in AllObjects)
            {
                var comp = go.GetComponentByID(componentID);

                if(comp != null)
                {
                    return comp;
                }
            }

            return null;
        }

        public T GetComponentByID<T>(string objectID, string componentID) where T : GrandmaComponent
        {
            return GetComponentByID(objectID, componentID) as T;
        }

        public GrandmaComponent GetComponentByID(string objectID, string componentID, Type componentType = null)
        {
            if (string.IsNullOrEmpty(objectID) || string.IsNullOrEmpty(componentID))
            {
                return null;
            }

            return GetByObjectID(objectID)?.GetComponentByID(componentID);
        }
        #endregion

        #region Object Creation
        public static T CreateNewComponent<T>(GameObject obj = null) where T : GrandmaComponent
        {
            return CreateNewComponent(typeof(T), obj) as T;
        }

        public static GrandmaComponent CreateNewComponent(Type t, GameObject obj = null)
        {
            if (t == null || t.IsSubclassOf(typeof(GrandmaComponent)) == false)
            {
                return null;
            }

            obj = obj ?? new GameObject("[Grandma]" + t.Name);

            GrandmaComponent comp = obj.AddComponent(t) as GrandmaComponent;

            return comp;
        }
        #endregion
    }
}
