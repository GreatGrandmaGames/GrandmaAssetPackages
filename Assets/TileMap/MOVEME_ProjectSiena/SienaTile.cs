using Grandma.Tiles;
using Grandma.Geometry;
using UnityEngine;

public class SienaTile : Tile
{
    private Biome biome;
    private PerturbablePlane tileTop;

    public Biome Biome
    {
        get
        {
            return biome;
        }
        set
        {
            biome = value;

            if (tileTop != null)
            {
                Destroy(tileTop);
            }

            tileTop = biome?.GenerateTileTop();

            if (tileTop != null)
            {
                tileTop.transform.SetParent(Prism.transform);
                tileTop.transform.localPosition = Vector3.zero;
                tileTop.transform.localRotation = Quaternion.identity;

                tileTop.Render();
            }
        }
    }

    public override void OnLinkedToTileMap(TileMap map)
    {
        base.OnLinkedToTileMap(map);

        //TODO: get biome
        Biome = FindObjectOfType<Biome>();
    }
}
