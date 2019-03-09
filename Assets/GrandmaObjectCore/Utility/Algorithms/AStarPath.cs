using System;
using System.Collections.Generic;
using UnityEngine;

namespace Grandma.Tiles
{
    public class AStarPath
    {
        public List<Tile> ValidPath { get; private set; }

        public AStarPath(TileMap tileMap, Tile startTile, Tile endTile)
        {
            ValidPath = FindPath(tileMap.AllTiles, startTile, endTile, tileMap.HeuristicNavigationEstimate);
        }

        private float DefaultEstimate(Tile a, Tile b)
        {
            return Vector3Int.Distance(a.Position, b.Position);
        }
        
        private List<Tile> FindPath(List<Tile> allTiles, Tile startTile, Tile endTile, Func<Tile, Tile, float> heuristicCostEstimate)
        {
            if(allTiles.Contains(startTile) == false || allTiles.Contains(endTile) == false)
            {
                Debug.LogWarning("AStarPath: Attempting to find path but the start / end path are not part of the graph");
                return null;
            }

            //Variable init
            Dictionary<Tile, Tile> path = new Dictionary<Tile, Tile>();

            List<Tile> closedSet = new List<Tile>();

            PriorityQueue<float, Tile> openSet = new PriorityQueue<float, Tile>();
            openSet.Enqueue(0, startTile);

            //G Score Init
            Dictionary<Tile, float> g_score = new Dictionary<Tile, float>();

            foreach (Tile h in allTiles)
            {
                g_score[h] = Mathf.Infinity;
            }

            g_score[startTile] = 0;

            //F score init
            Dictionary<Tile, float> f_score = new Dictionary<Tile, float>();

            foreach (Tile h in allTiles)
            {
                f_score[h] = Mathf.Infinity;
            }

            f_score[startTile] = heuristicCostEstimate(startTile, endTile);

            //Search loop
            while (!openSet.IsEmpty)
            {
                Tile current = openSet.Dequeue().Value;

                //path is complete
                if (current == endTile)
                {
                    return ProcessPath(path, startTile, endTile);
                }

                closedSet.Add(current);

                foreach (var n in current.LinkedAssociations)
                {
                    var nData = n.AssocData as NeighbourData;
                    var nTile = n.Component as Tile;

                    //If move cost is infinite, or tile is impassable
                    if (nData == null || nData.moveCost == Mathf.Infinity || (nData.passable.IsUnlocked == false))
                    {
                        continue;
                    }

                    if (nTile == null || closedSet.Contains(nTile))
                    {
                        continue;
                    }

                    float tentative_g_score = g_score[current] + nData.moveCost;

                    if (openSet.Contains(nTile))
                    {
                        //a shorter path has been found before
                        if (tentative_g_score >= g_score[nTile])
                        {
                            continue;
                        }
                    }

                    path[nTile] = current;

                    g_score[nTile] = tentative_g_score;
                    f_score[nTile] = g_score[nTile] + heuristicCostEstimate(nTile, endTile);

                    if (openSet.Contains(nTile) == false)
                    {
                        openSet.Enqueue(f_score[nTile], nTile);
                    }
                }
            }

            return null;
        }

        private List<Tile> ProcessPath(Dictionary<Tile, Tile> tiles, Tile start, Tile end)
        {
            var path = new List<Tile>
            {
                end
            };

            Tile current = end;

            while(tiles.ContainsKey(current))
            {
                path.Add(current);
                current = tiles[current];
            }

            path.Reverse();

            return path;
        }
    }
}