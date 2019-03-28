using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Grandma.GrandmaComponent;

namespace Grandma.Tiles
{
    public abstract class TileMapGenerator : MonoBehaviour
    {
        [SerializeField] protected Tile tilePrefab;
        [SerializeField] protected TileData waterTile;

        public Tile[,] Map { get; private set; }

        protected int Width { get; private set; }
        protected int Height { get; private set; }

        public void Init(int width, int height)
        {
            this.Width = width;
            this.Height = height;
            Map = CreateMapData();

            PlaceTerrain();
        }

        //Helper
        protected Tile CreateTile(TileData data)
        {
            Tile t = Instantiate(tilePrefab);

            //Back end
            //Set tile data - we want to provide initial data with some position
            t.initialDataMode = InitialDataMode.Provide;
            TileData tileData = data;

            t.initialData = tileData;

            t.Init();

            return t;
        }

        public Tile TileFor(int x, int y)
        {
            return Map[x, y];
        }

        protected abstract Tile[,] CreateMapData();
        protected abstract void PlaceTerrain();
    }
}

