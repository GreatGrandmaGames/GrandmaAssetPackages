using System;
using UnityEngine;

namespace Grandma.Tiles
{
    public class TileData : GrandmaCollectionData
    {
        public Vector3Int postion;
    }

    [Serializable]
    public class NeighbourData : GrandmaAssociationData
    {
        public LockDown passable = new LockDown();
        public float moveCost = 1f;

        public NeighbourData(string otherCompID) : base(otherCompID) { }
    }
}
