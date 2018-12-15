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
        public void Register(GrandmaObject gObj)
        {
            if (gObj == null)
            {
                Debug.LogError("GrandmaObjectManager -> Register(): Provided object is null");
                return;
            }

            //Data Verification
            if (gObj.data == null)
            {
                gObj.data = new GrandmaData();
            }

            gObj.data.id = idGen.RegisterID(gObj.data.id);

            //Default for name - Gameobjet's name
            if (string.IsNullOrEmpty(gObj.data.name))
            {
                gObj.data.name = gObj.name;
            }
            else
            {
                gObj.name = gObj.data.name;
            }
            //end Data Verification

            allObjects.Add(gObj);
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
            return allObjects.SingleOrDefault(x => x.data.id == id);
        }
        #endregion
    }
}
