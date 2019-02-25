using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grandma.Tiles
{
    public enum Direction
    {
        X,
        Y,
        Z
    }

    /// <summary>
    /// Alogirthms used in the Hex Map
    /// </summary>
    public class HexTileMap : TileMap
    {

        protected override Vector3Int CoordinatesFor(int x, int y)
        {
            return new Vector3Int(x, y, -x - y);
        }

        protected override List<Vector3Int> NeighbourCoordinates(Vector3Int center)
        {
            return new List<Vector3Int>
            {
                Step(center, 1, Direction.X),
                Step(center, -1, Direction.X),
                Step(center, 1, Direction.Y),
                Step(center, -1, Direction.Y),
                Step(center, 1, Direction.Z),
                Step(center, -1, Direction.Z)
            };
        }

        public Vector3Int Step(Vector3Int from, int by, Direction towards)
        {
            switch (towards)
            {
                case Direction.X:
                    return new Vector3Int(from.x, from.y + by, from.z - by);
                case Direction.Y:
                    return new Vector3Int(from.x + by, from.y, from.z - by);
                case Direction.Z:
                    return new Vector3Int(from.x + by, from.y - by, from.z);
                default:
                    return from;
            }
        }
    }
}
