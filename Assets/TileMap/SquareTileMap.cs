using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grandma.Tiles
{
    public class SquareTileMap : TileMap
    {
        protected override Vector3Int CoordinatesFor(int x, int y)
        {
            return new Vector3Int(x, y, 0);
        }

        protected override List<Vector3Int> NeighbourCoordinates(Vector3Int pos)
        {
            return new List<Vector3Int>
            {
                Step(pos, 1, Direction.X),
                Step(pos, -1, Direction.X),
                Step(pos, 1, Direction.Y),
                Step(pos, -1, Direction.Y),
            };
        }

        public Vector3Int Step(Vector3Int from, int by, Direction towards)
        {
            switch (towards)
            {
                case Direction.X:
                    return new Vector3Int(from.x + by, from.y, from.z);
                case Direction.Y:
                    return new Vector3Int(from.x, from.y + by, from.z);
                default:
                    return from;
            }
        }
    }
}
