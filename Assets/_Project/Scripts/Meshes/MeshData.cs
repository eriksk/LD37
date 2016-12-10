using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets._Project.Scripts.Meshes
{
    [Serializable]
    public class MeshData
    {
        public List<Vector3> Vertices = new List<Vector3>(); 
        public List<int> Indices = new List<int>(); 
        public List<Vector2> Uvs = new List<Vector2>(); 
        public List<Color32> Colors = new List<Color32>();

        public MeshData()
        {
        }

        public void Clear()
        {
            Vertices.Clear();
            Indices.Clear();
            Uvs.Clear();
            Colors.Clear();
        }

        public void ApplyTo(Mesh mesh)
        {
            mesh.Clear();

            mesh.SetVertices(Vertices);
            mesh.SetUVs(0, Uvs);
            mesh.SetIndices(Indices.ToArray(), MeshTopology.Triangles, 0);
            mesh.SetColors(Colors);

            mesh.RecalculateNormals();
            mesh.Optimize();
        }
    }
}
