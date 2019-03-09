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

    public class HexTileMap : TileMap
    {
        public override float HeuristicNavigationEstimate(Tile a, Tile b)
        {
            return Mathf.Max(Mathf.Abs(a.Position.x - b.Position.x),
                             Mathf.Abs(a.Position.y - b.Position.y),
                             Mathf.Abs(a.Position.z - b.Position.z));
        }

        public override Vector3Int Step(Vector3Int from, int by, Direction towards)
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

        public override Vector2 PositionToWorld(Vector3Int position)
        {
            Vector2Int axial = CubeToAxial(position);

            float sqrtThree = Mathf.Sqrt(3);
            float x = tileMapData.tileSize * (sqrtThree * axial.x + sqrtThree/2 * axial.y);
            float y = tileMapData.tileSize * (                      3f / 2      * axial.y);

            return new Vector2(x, y);
        }

        protected override Vector3Int CoordinatesFor(int x, int y)
        {
            return AxialToCube(x, y);
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

        #region Conversions
        public Vector2Int CubeToAxial(Vector3Int cube)
        {
            return new Vector2Int(cube.x, cube.z);
        }

        public Vector3Int AxialToCube(Vector2Int axial)
        {
            return AxialToCube(axial.x, axial.y);
        }

        public Vector3Int AxialToCube(int x, int y)
        {
            return new Vector3Int(x, y, -x-y);
        }
        #endregion
    }
}
