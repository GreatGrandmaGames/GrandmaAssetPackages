using System.Linq;
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

        public Tile TileAt(int x, int y)
        {
            return TileAt(new Vector3Int(x, y, 0));
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

        /*
         * TODO: 
        //Connected Components Algoritm
        public List<List<Tile>> ConnectedComponents()
        {
            int labelCount = 0;
            Dictionary<Tile, int> labels = new Dictionary<Tile, int>();

            List<List<int>> equalivalents = new List<List<int>>();

            ForEachWidthHeight((i, j) =>
            {
                Tile curr = TileAt(i, j);

                Tile prevHorz = TileAt(i, j - 1);
                Tile prevVert = TileAt(i - 1, j);
                int prevHorzID = prevHorz?.TileID ?? -1;
                int prevVertID = prevVert?.TileID ?? -1;

                //If Connected - i.e. is same type as tile one below / to left
                if (curr.TileID == prevHorzID || curr.TileID == prevVertID)
                {
                    labels[curr] = prevHorz != null ? labels[prevHorz] : labels[prevVert];

                    //If conflicted - i.e. below ID and left ID are different
                    if (prevHorzID != prevVertID)
                    {
                        //TODO: fix conflicted
                        foreach (var e in equalivalents)
                        {
                            if (e.Contains(prevHorzID))
                            {
                                e.Add(prevVertID);
                            }
                        }
                    }
                }
                else
                {
                    labels[curr] = labelCount;
                    labelCount++;
                }
            });

            Dictionary<List<int>, List<Tile>> components = new Dictionary<List<int>, List<Tile>>();

            foreach(var kv in labels)
            {
                equalivalents.ForEach(e =>
                {
                    if (e.Contains(kv.Value))
                    {
                        components[e].Add(kv.Key);
                        return;
                    }
                });
            }

            return components.Values.ToList();
        }
        */
    }
}
