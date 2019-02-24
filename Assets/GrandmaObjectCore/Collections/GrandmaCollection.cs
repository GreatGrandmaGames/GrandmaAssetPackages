using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Grandma
{
    /// <summary>
    /// Persistent collection of Grandma Components
    /// </summary>
    public class GrandmaCollection : GrandmaComponent
    {
        protected List<GrandmaComponent> linkedComponents = new List<GrandmaComponent>();

        public List<GrandmaComponent> LinkedComponents
        {
            get
            {
                return new List<GrandmaComponent>(linkedComponents);
            }
        }

        [NonSerialized]
        private GrandmaCollectionData colData;

        public override GrandmaComponentData Data
        {
            get => base.Data;

            protected set
            {
                base.Data = value;

                colData = value as GrandmaCollectionData;
            }
        }

        #region Read and Link / Unlink
        protected override void OnRead(GrandmaComponentData data)
        {
            base.OnRead(data);

            //Foreach id in the data
            foreach(var id in colData.LinkedComponentIDs)
            {
                //If we don't have the component linked, link it
                if(linkedComponents.FirstOrDefault(x => x.ComponentID == id) == null)
                {
                    Link(id);
                }
            }

            //For each linked component
            foreach(var comp in linkedComponents)
            {
                //If the ID is no longer in the data, unlink it
                if (colData.LinkedComponentIDs.Contains(comp.ComponentID) == false)
                {
                    Unlink(comp);
                }
            }
        }

        protected virtual void Link(string id)
        {
            var comp = GrandmaObjectManager.Instance.GetComponentByID(id);

            if(comp != null)
            {
                linkedComponents.Add(comp);
            } 
            else
            {
                Debug.LogWarning("GrandmaCollection " + GetType().Name + " on " + name + ": Given ID " + id + " but no equivalent component could be found");
            }
        }

        protected virtual void Unlink(GrandmaComponent col)
        {
            linkedComponents.Remove(col);
        }
        #endregion

        #region Write and Add / Remove
        protected virtual bool CanAdd(GrandmaAssociationData comp)
        {
            return comp != null;
        }

        public void Add(GrandmaAssociationData comp)
        {
            if (CanAdd(comp))
            {
                //If only one object is allowed in the list, and a matching objecti found, do not add the duplicate
                if(colData.singleObjectList && 
                    colData.AssociationData.FirstOrDefault(x => x.OtherComponentID == comp.OtherComponentID) != null)
                {
                    return;
                }

                colData.AssociationData.Add(comp);

                Refresh();
            }
        }

        public void Remove(GrandmaComponent comp)
        {
            colData.AssociationData.RemoveAll(x => x.OtherComponentID == comp.ComponentID);

            Refresh();
        }
        #endregion
    }
}
