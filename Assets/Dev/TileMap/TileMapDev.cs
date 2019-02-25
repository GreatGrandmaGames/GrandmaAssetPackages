using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Grandma;
using Grandma.Tiles;

public class TileMapDev : MonoBehaviour
{
    public TileMap map;
    public GameObject tempTilePrfab;

    private void Awake()
    {
        //t = GrandmaObjectManager.CreateNewComponent<Tile>();
    }

    private void Start()
    {
        Dictionary<Vector3Int, GameObject> tileObjs = new Dictionary<Vector3Int, GameObject>();

        foreach(var t in map.AllTiles)
        {
            var g = Instantiate(tempTilePrfab);
            g.transform.position = t.Position;

            tileObjs[t.Position] = g;
        }


        Tile start = map.AllTiles[0];
        Tile end = map.AllTiles[73];

        Debug.Log("ASTarPath Test: Starting at " + start + " and ending at " + end);

        var path = map.GetPath(start, end)?.ValidPath;

        for (int i = 0; i < path.Count - 1; i++ )
        {
            Debug.LogFormat("Current {0} Next {1} Diff {2}", path[i].Position, path[i + 1].Position, path[i + 1].Position - path[1].Position);

            tileObjs[path[i].Position].GetComponentInChildren<Renderer>().material.color = Color.red;
        }

        //tileObjs[start.Position].GetComponentInChildren<Renderer>().material.color = Color.blue;
        //tileObjs[end.Position].GetComponentInChildren<Renderer>().material.color = Color.green;


        /*
        Debug.Log("Pre add");
        Debug.Log(t.TileConnections.LinkedComponents.Count);
        Debug.Log(t.TileConnections.Data);

        t.TileConnections.Add(new GrandmaAssociationData() {
           OtherComponentID = tile2.ComponentID,

        });

        tile2.TileConnections.Add(new GrandmaAssociationData()
        {
            OtherComponentID = t.ComponentID,
        });

        Debug.Log("Pre write");
        Debug.Log(t.TileConnections.LinkedComponents.Count);
        Debug.Log(t.TileConnections.Data);

        t.TileConnections.Refresh();

        Debug.Log("Post wrtie");
        Debug.Log(t.TileConnections.LinkedComponents.Count);
        Debug.Log(t.TileConnections.Data);
        */
    }

}
