using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Grandma.Tiles
{
    public abstract class TileMap : GrandmaCollection
    {
        //Inspector
        [SerializeField] protected TileMapGenerator generator;

        //Private variables
        private Dictionary<Vector3Int, Tile> tilesForPosition = new Dictionary<Vector3Int, Tile>(new TilePositionComparator());
        [NonSerialized]
        protected TileMapData tileMapData;

        public int Width
        {
            get
            {
                return tileMapData.width;
            }
        }

        public int Height
        {
            get
            {
                return tileMapData.height;
            }
        }

        //Properties
        public override GrandmaComponentData Data
        {
            get => base.Data;

            protected set
            {
                base.Data = value;

                tileMapData = Data as TileMapData;
            }
        }

        public List<Tile> AllTiles
        {
            get
            {
                return LinkedComponents.OfType<Tile>().ToList();
            }
        }

        #region Grandma Collection Overrides
        public override bool CanAssociate(GrandmaComponent comp)
        {
            return base.CanAssociate(comp) && comp as Tile != null;
        }

        protected override void Link(LinkedAssociation comp)
        {
            base.Link(comp);

            Tile t = comp.Component as Tile;

            if(t != null)
            {
                tilesForPosition[t.Position] = t;

                t.transform.position = PositionToWorld(t.Position);
                t.TileMap = this;

                t.Refresh();
            }
        }

        protected override void Unlink(LinkedAssociation comp)
        {
            base.Unlink(comp);

            Tile t = comp.Component as Tile;

            if (t != null)
            {
                tilesForPosition.Remove(t.Position);
            }
        }
        #endregion

        public Tile TileAt(Vector3Int position)
        {
            return tilesForPosition.ContainsKey(position) ? tilesForPosition[position]: null;
        }

        public void ForEachWidthHeight(Action<int, int> action)
        {
            IterationUtility.ForEach2D(tileMapData.width, tileMapData.height, action);
        }

        //TODO: allow for HCE
        public AStarPath GetPath(Tile start, Tile end)
        {
            return new AStarPath(this, start, end);
        }

        #region Creation
        protected override void OnCreated()
        {
            base.OnCreated();

            generator.Init(tileMapData.width, tileMapData.height);

            //Create tiles
            List<GrandmaAssociationData> tileData = new List<GrandmaAssociationData>();

            ForEachWidthHeight((i, j) =>
            {
                tileData.Add(new GrandmaAssociationData(generator.TileFor(i, j).ComponentID));
            });

            Add(tileData);

            Refresh();

            //Link Neighbours
            foreach (Tile t in AllTiles)
            {
                List<GrandmaAssociationData> nData = new List<GrandmaAssociationData>();

                foreach (Vector3Int v in NeighbourCoordinates(t.Position))
                {
                    Tile otherT = TileAt(v);

                    if (otherT != null)
                    {
                        nData.Add(new NeighbourData(otherT.ComponentID));
                    }
                }

                t.Add(nData);
            }
        }
        #endregion

        #region Abstract
        //Navigation
        /// <summary>
        /// For A* navigation
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public abstract float HeuristicNavigationEstimate(Tile a, Tile b);

        public abstract Vector3Int Step(Vector3Int from, int by, Direction towards);

        public Vector2 PositionToWorld(Tile t)
        {
            return PositionToWorld(t.Position);
        }
        public abstract Vector2 PositionToWorld(Vector3Int position);

        /// <summary>
        /// Converts (x, y) initial co-ordinate into Position
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        protected abstract Vector3Int CoordinatesFor(int x, int y);

        /// <summary>
        /// Get all neighoubr's co-ordinates for fast traversal of the tilemap
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        protected abstract List<Vector3Int> NeighbourCoordinates(Vector3Int pos);

        
        #endregion
    }
}

