using UnityEngine;
using System.Linq;

namespace Grandma.Geometry
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public abstract class Renderable : MonoBehaviour
    {
        private MeshFilter meshFilter;

        public bool renderOnAwake;
        public bool useCollider;

        private void Awake()
        {
            if (renderOnAwake)
            {
                Render();
            }
        }

        public void Render()
        {
            var mesh = GenerateMesh();

            if (meshFilter == null)
            {
                meshFilter = GetComponent<MeshFilter>();
            }

            meshFilter.sharedMesh = mesh;

            if (useCollider)
            {
                var mc = GetComponent<MeshCollider>();

                if (mc == null)
                {
                    mc = gameObject.AddComponent<MeshCollider>();
                }

                mc.sharedMesh = mesh;
            }
        }

        protected abstract Mesh GenerateMesh();

        protected int[] FillSurface(int[] verts, bool clockwise)
        {
            int numVerts = verts.Length;
            int[] triangles = new int[0];

            //Case where we have a square in the previous call
            if (numVerts <= 2)
            {
                return new int[0];
            }

            if (numVerts == 3)
            {

                if (clockwise)
                {
                    return new int[]
                    {
                    verts[0], verts[2], verts[1]
                    };
                }
                else
                {
                    return verts;
                }
            }

            int[] unfinished = new int[((numVerts + 1) / 2)];

            for (int i = 0; i < numVerts; i += 2)
            {
                unfinished[i / 2] = verts[i];

                if (i + 1 < numVerts)
                {
                    int[] triangle;

                    if (clockwise)
                    {
                        triangle = new int[]
                        {
                    verts[i], (verts[(i+ 2) % numVerts]), verts[i + 1]
                        };
                    }
                    else
                    {
                        triangle = new int[]
                        {
                    verts[i], verts[i + 1], (verts[(i + 2) % numVerts])
                        };
                    }

                    triangles = triangles.Concat(triangle).ToArray();
                }
            }


            return triangles.Concat(FillSurface(unfinished, clockwise)).ToArray();
        }

        protected Vector2[] PointsAboutEllipse(int numIncrements, TopType topType, float verticalSquash = 1f)
        {
            var corners = new Vector2[numIncrements];

            float aOffset = 0;

            if (topType == TopType.Flat)
            {
                aOffset = 0.5f;
            }

            for (int inc = 0; inc < numIncrements; inc++)
            {
                float theta = ((2f * Mathf.PI) * ((inc + aOffset) / (float)numIncrements));

                corners[inc] = new Vector2(Mathf.Cos(theta), Mathf.Sin(theta) * verticalSquash);
            }

            return corners;
        }
    }

    public enum FaceType
    {
        Flat,
        Round
    }

    public enum TopType
    {
        Pointy,
        Flat
    }
}
