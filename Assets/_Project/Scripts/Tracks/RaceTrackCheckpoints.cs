using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets._Project.Scripts.Paths;
using UnityEngine;
using Assets._Project.Scripts.Game;

namespace Assets._Project.Scripts.Tracks
{
    [RequireComponent(typeof(RaceTrack))]
    public class RaceTrackCheckpoints : MonoBehaviour
    {
        public float Resolution = 1f;

        void Start()
        {
            var track = GetComponent<RaceTrack>();
            int number = 0;
            track.Track.IterateSpline((info) => 
            {
                AddCheckpoint(track, info, number++);
            }, Resolution);
        }

        private void AddCheckpoint(RaceTrack track, IterableSplineInfo info, int number)
        {
            var checkpoint = new GameObject("Checkpoint_" + number);
            checkpoint.transform.parent = transform;
            var cp = checkpoint.AddComponent<Checkpoint>();
            cp.Number = number;

            checkpoint.transform.position = info.Position;
            cp.GetComponent<SphereCollider>().radius = (track.TrackWidth * 1.2f) * 0.5f;
        }
    }
}
