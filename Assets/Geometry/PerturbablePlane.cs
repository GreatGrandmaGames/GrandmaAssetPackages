using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Grandma.Geometry
{
    public class PerturbablePlane : Renderable
    {
        [Header("Geometry")]
        [Range(3, 60)]
        public int vertCount = 6;

        [Header("Shape")]
        public TopType topType = TopType.Flat;
        public float radius = 1f;

        protected override Mesh GenerateMesh()
        {
            Vector3[] vertices = new Vector3[vertCount];

            var corners = PointsAboutEllipse(vertCount, topType);

            for (int i = 0; i < vertCount; i++)
            {
                vertices[i] = new Vector3((corners[i].x) * radius, 0f, corners[i].y * radius);
            }

            List<int> triangles = FillSurface(Enumerable.Range(0, vertCount).ToArray(), true).ToList();

            Vector3[] normals = new Vector3[vertices.Length];

            for (int i = 0; i < vertCount; i++)
            {
                normals[i] = Vector3.up;
            }

            Mesh mesh = new Mesh
            {
                name = "PertuablePlane (" + vertCount + ")",
                vertices = vertices,
                normals = normals,
                triangles = triangles.ToArray()
            };

            return mesh;
        }
    }
}
