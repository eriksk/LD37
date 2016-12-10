using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets._Project.Scripts.Cars.Input
{
    [RequireComponent(typeof(Car))]
    public class CarPlayerInputController : MonoBehaviour
    {
        private Car _car;

        void Start()
        {
            _car = GetComponent<Car>();
        }

        void Update()
        {
            var v = UnityEngine.Input.GetAxis("Vertical");
            var h = UnityEngine.Input.GetAxis("Horizontal");

            var motor = v > 0f ? v : 0f;
            var brakes = v < 0f ? -v : 0f;
            var steering = h;

            _car.UpdateInput(steering, motor, brakes);
        }
    }
}
