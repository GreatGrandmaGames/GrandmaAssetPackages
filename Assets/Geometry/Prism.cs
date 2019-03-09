using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Grandma.Geometry
{
    public class Prism : Renderable
    {
        [Header("Geometry")]
        [Range(3, 60)]
        public int vertCount = 6;
        public Vector2 shear = Vector2.zero;
        public Vector2 frustumScale = Vector2.one;
        public float truncationAngle = 0f;
        public float verticalSquash = 1f;

        [Header("Shape")]
        public FaceType faceType = FaceType.Flat;
        public TopType topType = TopType.Flat;
        public float thickness = 0.5f;
        public float radius = 1f;

        [Header("Prism Options")]
        public bool hideTop;
        public bool showInsides;
        public List<int> hiddenSideFaces;

        protected override Mesh GenerateMesh()
        {
            int normalsPerVert = 0;

            //When we have flat faces, we need three normals per point
            if (faceType == FaceType.Flat)
            {
                normalsPerVert = 3;
            }
            //When we have round faces, we only need two normals per point
            else if (faceType == FaceType.Round)
            {
                normalsPerVert = 2;
            }

            //The number of geometric vertices for a prism is 2 * vertCount. We multiple by vertsPerPoint for normals
            Vector3[] vertices = new Vector3[2 * vertCount * normalsPerVert];

            var corners = PointsAboutEllipse(vertCount, topType, verticalSquash);

            for (int i = 0; i < vertCount; i++)
            {
                for (int j = 0; j < normalsPerVert; j++)
                {
                    int k1 = (i * normalsPerVert) + j;
                    int k2 = k1 + (vertCount * normalsPerVert);

                    vertices[k1] = new Vector3((corners[i].x) * radius, 0f, corners[i].y * radius);

                    float bottomVertHeight = Mathf.Tan(truncationAngle) * corners[i].x * radius;

                    vertices[k2] = new Vector3((corners[i].x + shear.x) * frustumScale.x * radius, -thickness - bottomVertHeight, (corners[i].y + shear.y) * frustumScale.y * radius);
                }
            }

            List<int> triangles = new List<int>();

            if (hideTop == false)
            {
                triangles.AddRange(FillSurface(Enumerable.Range(0, (vertices.Length / 2) - 1).Where(x => (x % normalsPerVert == 0)).ToArray(), true));
            }

            triangles.AddRange(FillSurface(Enumerable.Range((vertices.Length / 2), (vertices.Length / 2) - 1).Where(x => (x % normalsPerVert == 0)).ToArray(), false));

            Vector3[] normals = new Vector3[vertices.Length];

            for (int i = 0; i < vertCount; i++)
            {
                int cornerVertIndex = i * normalsPerVert;
                int bottomFaceEquivalent = (vertCount * normalsPerVert);

                normals[cornerVertIndex] = Vector3.up;
                normals[cornerVertIndex + bottomFaceEquivalent] = new Vector3(-Mathf.Sin(truncationAngle), -Mathf.Cos(truncationAngle), 0).normalized;

                int topLeft = cornerVertIndex + 1;
                int bottomLeft = topLeft + bottomFaceEquivalent;

                //Couldn't see a meaningful function for this
                int normalSkip = (normalsPerVert == 3) ? 4 : 2;

                int topRight = (topLeft + normalSkip) % (vertices.Length / 2);
                int bottomRight = topRight + bottomFaceEquivalent;

                int[] sideFace = new int[]
                {
                topLeft,
                bottomLeft,
                bottomRight,
                topRight
                };

                //if we have elected to hide this face, don't create triangles
                //else, do
                if (hiddenSideFaces != null && hiddenSideFaces.Contains(i) == false)
                {
                    triangles.AddRange(FillSurface(sideFace, true));
                }

                if (faceType == FaceType.Flat)
                {
                    float angle = (2f * Mathf.PI) * (i / (float)vertCount) + (Mathf.PI / vertCount) + (topType == TopType.Flat ? (Mathf.PI / vertCount) : 0f);

                    var circlePoint = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

                    Vector3 normalVec = new Vector3(circlePoint.x, 0f, circlePoint.y).normalized;

                    foreach (int s in sideFace)
                    {
                        normals[s] = normalVec;
                    }

                }
                else if (faceType == FaceType.Round)
                {
                    //If we want rounded edges, use the vector for this corner
                    //The line that travels through the center of the prism face to the corner
                    Vector3 normalVec = new Vector3(corners[i].x, 0f, corners[i].y).normalized;

                    normals[topLeft] = normalVec;
                    normals[bottomLeft] = normalVec;
                }
            }

            if (showInsides)
            {
                int[] insideTriangles = new int[triangles.Count];

                for (int i = 0; i < triangles.Count; i += 3)
                {
                    insideTriangles[i] = triangles[i];
                    insideTriangles[i + 1] = triangles[i + 2];
                    insideTriangles[i + 2] = triangles[i + 1];
                }

                triangles.AddRange(insideTriangles);
            }

            Mesh mesh = new Mesh
            {
                name = "Prism (" + vertCount + ")",
                vertices = vertices,
                normals = normals,
                triangles = triangles.ToArray()
            };

            return mesh;
        }
    }
}