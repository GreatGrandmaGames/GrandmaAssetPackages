using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

using Grandma.Geometry;

namespace Grandma.Tiles
{
    public class Tile : GrandmaCollection
    {
        //Inspector variables
        public Prism Prism;

        //Private variable
        [NonSerialized]
        protected TileData tileData;

        //Properties
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

        public virtual void OnLinkedToTileMap(TileMap map)
        {
            transform.position = map.PositionToWorld(Position);

            //Set the radius of the tile to the size specified by the map
            Prism.radius = (map.Data as TileMapData)?.tileSize ?? 0f;

            Prism.Render();
        }
    }
}
