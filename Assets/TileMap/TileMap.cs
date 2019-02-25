using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Grandma.Tiles
{
    public abstract class TileMap : GrandmaCollection
    {
        //Private variables
        private Dictionary<Vector3Int, Tile> tilesForPosition = new Dictionary<Vector3Int, Tile>();
        [NonSerialized]
        protected TileMapData tileMapData;

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
            Debug.Log(position);

            return tilesForPosition.ContainsKey(position) ? tilesForPosition[position] : null;
        }

        //TODO: allow for HCE
        public AStarPath GetPath(Tile start, Tile end)
        {
            return new AStarPath(AllTiles, start, end);
        }

        #region Creation
        protected override void OnCreated()
        {
            base.OnCreated();

            //Create tiles
            for (int x = -(tileMapData.width / 2); x < (tileMapData.width / 2); x++)
            {
                for (int y = -(tileMapData.height / 2); y < (tileMapData.height / 2); y++)
                {
                    AddToData(new GrandmaAssociationData(CreateTile(x, y).ComponentID));
                }
            }

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

        private Tile CreateTile(int x, int y)
        {
            Tile t = GrandmaObjectManager.CreateNewComponent<Tile>();

            //Set tile data - we want to provide initial data with some position
            t.initialDataMode = InitialDataMode.Provide;
            TileData tileData = ScriptableObject.CreateInstance(typeof(TileData)) as TileData;
            //Cube co-ordinate
            tileData.postion = CoordinatesFor(x, y);

            t.initialData = tileData;

            t.Init();

            t.gameObject.name = "Tile At " + t.Position;

            return t;
        }

        protected abstract Vector3Int CoordinatesFor(int x, int y);
        protected abstract List<Vector3Int> NeighbourCoordinates(Vector3Int pos);
        #endregion
    }
}

