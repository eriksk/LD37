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
            var meshData = RaceTrackBuilder.Build(SegmentMaterial, this, Resolution, Thickness);

            var meshFilter = GetComponent<MeshFilter>() ?? gameObject.AddComponent<MeshFilter>();
            var meshRenderer = GetComponent<MeshRenderer>() ?? gameObject.AddComponent<MeshRenderer>();
            meshRenderer.sharedMaterial = SegmentMaterial;
            var mesh = meshFilter.sharedMesh ?? new Mesh() { name = name + "_mesh" };
            meshData.ApplyTo(mesh);
            meshFilter.sharedMesh = mesh;
        }

    }
}
