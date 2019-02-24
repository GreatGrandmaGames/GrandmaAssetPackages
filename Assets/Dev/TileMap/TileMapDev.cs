using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Grandma;
using Grandma.Tiles;

public class TileMapDev : MonoBehaviour
{
    public Tile tile2;

    private Tile t;

    private void Awake()
    {
        t = GrandmaObjectManager.Instance.CreateNewComponent<Tile>();
    }

    private void Start()
    {
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
    }
}
