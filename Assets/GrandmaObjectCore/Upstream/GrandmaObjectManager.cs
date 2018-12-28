using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Grandma.Core
{
    public class GrandmaObjectManager : MonoBehaviour
    {
        //Variables
        private IDGenerator idGen;
        private List<GrandmaObject> allObjects = new List<GrandmaObject>();

        #region Singleton
        public static GrandmaObjectManager Instance { get; private set; }
        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogWarning("GrandmaObjectManager: Can only have one instance per scene. Destroying this...");
                Destroy(this);
            }

            Instance = this;

            idGen = new IDGenerator();
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

            return new GrandmaObjectData(idGen.NewID(), gObj.name);
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

        #region Object Retrival
        public List<GrandmaObject> AllObjects
        {
            get
            {
                var cloneList = new List<GrandmaObject>();
                cloneList.AddRange(allObjects);
                return cloneList;
            }
        }

        public GrandmaObject GetByID(string id)
        {
            return allObjects.SingleOrDefault(x => x.Data.id == id);
        }

        public GrandmaComponent GetComponentByID(string id, Type componentType)
        {
            if (componentType.IsSubclassOf(typeof(GrandmaComponent)) == false)
            {
                //Not a grandmaable component
                return null;
            }

            var obj = GetByID(id);

            if(obj != null)
            {
                return obj.GetComponent(componentType) as GrandmaComponent;
            } else
            {
                return null;
            }
        }
        #endregion
    }
}
