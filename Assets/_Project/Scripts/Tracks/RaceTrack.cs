using Assets._Project.Scripts.Meshes;
using Assets._Project.Scripts.Paths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets._Project.Scripts.Tracks
{
    [RequireComponent(typeof(CatmullRomSpline))]
    public class RaceTrack : MonoBehaviour
    {
        public Material SegmentMaterial;
        public float TrackWidth = 4f;
        public float Resolution = 0.1f;
        public float Thickness = 2f;
        public float SideThickness = 2f;

        public PhysicMaterial TrackMaterial;
        public PhysicMaterial TrackSidesMaterial;

        private CatmullRomSpline _track;
        public CatmullRomSpline Track
        {
            get
            {
                return _track ?? (_track = GetComponent<CatmullRomSpline>());
            }
        }

        public void Build()
        {
            var meshData = RaceTrackBuilder.Build(SegmentMaterial, this, Resolution, Thickness, SideThickness);

            DestroyTrackMeshes();

            CreateTrackMeshChild("Track", meshData.Track, TrackMaterial);
            //CreateTrackMeshChild("Track Sides", meshData.Sides, TrackSidesMaterial);

        }

        private void CreateTrackMeshChild(string name, MeshData meshData, PhysicMaterial material)
        {
            var trackMeshObject = new GameObject(name);
            var meshFilter = trackMeshObject.AddComponent<MeshFilter>();
            var meshRenderer = trackMeshObject.AddComponent<MeshRenderer>();
            meshRenderer.sharedMaterial = SegmentMaterial;

            var mesh = meshFilter.sharedMesh ?? new Mesh() { name = name + "_mesh" };
            meshData.ApplyTo(mesh);
            meshFilter.sharedMesh = mesh;

            var meshCollider = trackMeshObject.AddComponent<MeshCollider>();
            meshCollider.sharedMesh = mesh;
            meshCollider.sharedMaterial = material;
            trackMeshObject.transform.parent = transform;
        }

        private void DestroyTrackMeshes()
        {
            var children = GetComponentsInChildren<MeshCollider>().Select(x => x.gameObject).ToArray();
            for (int i = 0; i < children.Length; i++)
                DestroyImmediate(children[i]);
        }
    }
}
