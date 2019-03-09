using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Grandma.Geometry;

public class ExampleBiome : Biome
{
    public PerturbablePlane example;

    public override PerturbablePlane GenerateTileTop()
    {
        return Instantiate(example);
    }
}
