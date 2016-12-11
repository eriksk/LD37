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

        void Start()
        {
            _car = GetComponent<Car>();
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
    }
}
