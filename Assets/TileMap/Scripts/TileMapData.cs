using System;
using System.Collections.Generic;
using UnityEngine;

namespace Grandma.Tiles
{
    [Serializable]
    [CreateAssetMenu(menuName = "Tiles/Tile Map Data")]
    public class TileMapData : GrandmaCollectionData
    {
        public int width;
        public int height;

        public float tileSize = 1f;
    }
}
