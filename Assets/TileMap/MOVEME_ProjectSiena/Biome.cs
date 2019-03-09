using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Grandma.Geometry;

//Pre-defined (i.e. don't vary by game), and so do not need to be saved
public abstract class Biome : MonoBehaviour
{
    public abstract PerturbablePlane GenerateTileTop();
}

