using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Grandma.Tiles
{
    public class Tile : GrandmaCollection
    {
        //Inspector variables
        [SerializeField] protected SpriteRenderer sRenderer;

        //Private variable
        [NonSerialized]
        private TileData tileData;

        //Properties
        public TileMap TileMap { get; set; }

        public override GrandmaComponentData Data {

            get => base.Data;

            protected set
            {
                base.Data = value;

                tileData = Data as TileData;
            }
        }

        public Vector3Int Position
        {
            get
            {
                return tileData.position;
            }
        }

        public List<Tile> Neighbours
        {
            get
            {
                return LinkedComponents.OfType<Tile>().ToList();
            }
        }

        public int TileID
        {
            get
            {
                return tileData.tileID;
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
