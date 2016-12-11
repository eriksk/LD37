using Assets._Project.Scripts.Tracks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets._Project.Scripts.Cars
{
    public abstract class CarController : ScriptableObject
    {
        public abstract void UpdateControl(Car car, RaceTrack track);
    }
}
