using System.Collections;
using System.Collections.Generic;
using Grandma.Geometry;
using UnityEngine;

namespace Grandma.Tiles
{
    public class SquareTileMap : TileMap
    {
        public override float HeuristicNavigationEstimate(Tile a, Tile b)
        {
            return Vector3.Distance(a.Position, b.Position);
        }

        public override Vector3Int Step(Vector3Int from, int by, Direction towards)
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

        public override Vector2 PositionToWorld(Vector3Int position)
        {
            return new Vector2(position.x, position.y);
        }

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
    }
}
