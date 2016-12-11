using Assets._Project.Scripts.Tracks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets._Project.Scripts.Game
{
    [RequireComponent(typeof(Car))]
    public class RaceProgressState : MonoBehaviour
    {
        private Car _car;
        public int CurrentCheckPoint = 0;
        public int LapCount = 0;
        public int NumberOfLaps = 2;

        private RaceTrackCheckpoints _checkpoints;

        public float RaceProgress
        {
            get
            {
                if (_checkpoints == null) return 0;

                float oneSegmentCountsAs = 1000f;

                float total = 0f;

                // previous laps
                total += (LapCount * _checkpoints.Checkpoints) * oneSegmentCountsAs;
                total += CurrentCheckPoint * oneSegmentCountsAs;

                var current = _checkpoints.GetCheckpoint(CurrentCheckPoint);
                //var next = _checkpoints.GetNextCheckpoint(CurrentCheckPoint);

                var myDistance = Vector3.Distance(transform.position, current.transform.position);

                total += myDistance;

                return total;
            }
        }

        void Start()
        {
            _car = GetComponent<Car>();
            _checkpoints = _car.Track.GetComponent<RaceTrackCheckpoints>();
        }

        void Update()
        {


        }

        void OnTriggerEnter(Collider collider)
        {
            var checkpoint = collider.gameObject.GetComponent<Checkpoint>();
            if (checkpoint == null) return;

            if(checkpoint.Number == CurrentCheckPoint + 1 || checkpoint.Number == 0)
                CurrentCheckPoint = checkpoint.Number;
            if(checkpoint.Number == 0)
            {
                LapCount++;
            }
        }

        public Checkpoint GetCurrentResetCheckpoint()
        {
            return _checkpoints.GetCheckpoint(CurrentCheckPoint);
        }
        public Checkpoint GetNextCheckpoint()
        {
            return _checkpoints.GetNextCheckpoint(CurrentCheckPoint);
        }
    }
}
