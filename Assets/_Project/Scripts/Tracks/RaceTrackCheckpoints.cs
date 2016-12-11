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

        private List<Checkpoint> _checkpoints;

        public int Checkpoints
        {
            get { return _checkpoints.Count; }
        }

        void Start()
        {
            _checkpoints = new List<Checkpoint>();
            var track = GetComponent<RaceTrack>();
            int number = 0;

            foreach (var node in track.Track.Nodes)
                AddCheckpoint(track, node.position, number++);
        }

        public Checkpoint GetNextCheckpoint(int current)
        {
            if (current == Checkpoints - 1)
                return _checkpoints.FirstOrDefault(x => x.Number == 0);
            return _checkpoints.FirstOrDefault(x => x.Number == current + 1);
        }

        public Checkpoint GetCheckpoint(int number)
        {
            return _checkpoints.FirstOrDefault(x => x.Number == number);
        }

        private void AddCheckpoint(RaceTrack track, Vector3 position, int number)
        {
            var checkpoint = new GameObject("Checkpoint_" + number);
            checkpoint.transform.parent = transform;
            var cp = checkpoint.AddComponent<Checkpoint>();
            cp.Number = number;

            checkpoint.transform.position = position;
            cp.GetComponent<SphereCollider>().radius = (track.TrackWidth * 1.2f) * 0.5f;

            _checkpoints.Add(cp);
        }
    }
}
