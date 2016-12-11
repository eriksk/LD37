using Assets._Project.Scripts.Cameras;
using Assets._Project.Scripts.Game;
using Assets._Project.Scripts.Tracks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

namespace Assets._Project.Scripts.Cars
{
    public class CarConfigurations : MonoBehaviour
    {
        public StartPositions StartPositions;
        public Camera Camera;
        public CarConfiguration[] Cars;
        public RaceTrack Track;

        public void Start()
        {
            if (Cars == null) return;

            var startPositions = StartPositions.Positions;
            int i = 0;
            foreach (var carConfig in Cars)
            {
                if (carConfig.IgnoreMe) continue;

                var prefab = carConfig.CarPrefab;

                var start = startPositions[i];
                var car = (GameObject)Instantiate(prefab, start.Position, start.Rotation);
                var carComponent = car.GetComponent<Car>();
                carComponent.Track = Track;
                carComponent.Controller = carConfig.Controller;
                if (carConfig.OwnsCamera)
                {
                    Camera.GetComponent<FollowCam>().Target = car.transform;
                    Camera.GetComponent<DepthOfField>().focalTransform = car.transform;
                }
                i++;
            }
        }
    }

    [Serializable]
    public class CarConfiguration
    {
        public GameObject CarPrefab;
        public CarController Controller;
        public bool OwnsCamera = false;
        public bool IgnoreMe = false;
    }
}
