using Assets._Project.Scripts.Tracks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets._Project.Scripts.Cars.Input
{
    [CreateAssetMenu(fileName = "Custom/Cars/Player Controller")]
    public class CarPlayerInputController : CarController
    {
        public override void UpdateControl(Car car, RaceTrack track)
        {
            var v = UnityEngine.Input.GetAxis("Vertical");
            var h = UnityEngine.Input.GetAxis("Horizontal");

            var motor = v > 0f ? v : 0f;
            var brakes = v < 0f ? -v : 0f;
            var steering = h;

            car.UpdateInput(steering, motor, brakes);
        }
    }
}
