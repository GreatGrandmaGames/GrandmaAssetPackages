using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Grandma
{
    /// <summary>
    /// Persistent collection of Grandma Components
    /// </summary>
    public class GrandmaCollection : GrandmaComponent
    {
        public class LinkedAssociation
        {
            //Component.ComponentID and AssocData.OtherComponentID are equal
            public GrandmaComponent Component { get; private set; }
            public GrandmaAssociationData AssocData { get; private set; }

            public LinkedAssociation(GrandmaComponent component, GrandmaAssociationData assocData)
            {
                this.Component = component;
                this.AssocData = assocData;
            }
        }

        protected List<LinkedAssociation> linkedComponents = new List<LinkedAssociation>();

        public List<GrandmaComponent> LinkedComponents
        {
            get
            {
                return linkedComponents.Select(x => x.Component).ToList();
            }
        }

        public List<LinkedAssociation> LinkedAssociations
        {
            get
            {
                return new List<LinkedAssociation>(linkedComponents);
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
            foreach(var assoc in colData.AssociationData)
            {
                //If we don't have the component linked, link it
                if(linkedComponents.FirstOrDefault(x => x.Component.ComponentID == assoc.OtherComponentID) == null)
                {
                    var comp = GrandmaObjectManager.Instance.GetComponentByID(assoc.OtherComponentID);

                    if (comp != null)
                    {
                        Link(new LinkedAssociation(comp, assoc));
                    }
                    else
                    {
                        Debug.LogWarning("GrandmaCollection " + GetType().Name + " on " + name + ": Given ID " + assoc.OtherComponentID + " but no equivalent component could be found");
                    }
                }
            }

            //For each linked component
            foreach(var comp in linkedComponents)
            {
                //If the ID is no longer in the data, unlink it
                if (colData.LinkedComponentIDs.Contains(comp.Component.ComponentID) == false)
                {
                    Unlink(comp);
                }
            }
        }

        protected virtual void Link(LinkedAssociation linkedAssoc)
        {
            linkedComponents.Add(linkedAssoc);
        }

        protected virtual void Unlink(LinkedAssociation unlinkedAssoc)
        {
            linkedComponents.Remove(unlinkedAssoc);
        }
        #endregion

        #region Write and Add / Remove
        /// <summary>
        /// Can a provided object be linked to this collection?
        /// </summary>
        /// <param name="comp"></param>
        /// <returns></returns>
        // This is optionally checked by the caller
        public virtual bool CanAssociate(GrandmaComponent comp)
        {
            return comp != null;
        }

        /// <summary>
        /// Can a provided association class be added to this collection?
        /// </summary>
        /// <param name="comp"></param>
        /// <returns></returns>
        //This is checked on Add
        protected virtual bool CanAdd(GrandmaAssociationData comp)
        {
            return comp != null;
        }
        
        public void Add(List<GrandmaAssociationData> comps)
        {
            bool update = false;
            comps.ForEach(x => update |= AddToData(x));

            if (update)
            {
                Refresh();
            }
        }

        public void Add(GrandmaAssociationData comp)
        {
            if (AddToData(comp))
            {
                Refresh();
            }
        }

        /// <summary>
        /// Adds the association to the data, without refreshing
        /// </summary>
        /// <param name="comp"></param>
        /// <returns></returns>
        // Allows lots of things to be added, and then refresh just once at the end
        protected bool AddToData(GrandmaAssociationData comp)
        {
            if (CanAdd(comp))
            {
                //If only one object is allowed in the list, and a matching objecti found, do not add the duplicate
                if (colData.singleObjectList &&
                    colData.AssociationData.FirstOrDefault(x => x.OtherComponentID == comp.OtherComponentID) != null)
                {
                    return false;
                }

                colData.AssociationData.Add(comp);
                return true;
            }

            return false;
        }

        public void Remove(GrandmaComponent comp)
        {
            colData.AssociationData.RemoveAll(x => x.OtherComponentID == comp.ComponentID);

            Refresh();
        }
        #endregion
    }
}
