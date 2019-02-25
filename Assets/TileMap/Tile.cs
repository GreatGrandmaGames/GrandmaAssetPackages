using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Grandma.Tiles
{
    public class Tile : GrandmaCollection
    {
        [NonSerialized]
        private TileData tileData;

        public override GrandmaComponentData Data {

            get => base.Data;

            protected set {
                base.Data = value;

                tileData = Data as TileData;
            }
        }

        public Vector3Int Position
        {
            get
            {
                return tileData.postion;
            }
        }

        public List<Tile> Neighbours
        {
            get
            {
                return LinkedComponents.OfType<Tile>().ToList();
            }
        }

        #region Collection (Neighbour) Overrides
        public override bool CanAssociate(GrandmaComponent comp)
        {
            return base.CanAssociate(comp) && comp as Tile != null;
        }
        #endregion
    }
}
