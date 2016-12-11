using Assets._Project.Scripts.Tracks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets._Project.Scripts.Cars.Input
{
    [CreateAssetMenu(fileName = "Custom/Cars/AI Controller")]
    public class CarAIInputController : CarController
    {
        public override void UpdateControl(Car car, RaceTrack track)
        {
            var lookAhead = 8;

            var paths = track.GetComponent<RaceTrackPaths>();

            var path = paths.Paths.First();

            var nearestIndex = path.GetNearestIndex(car.transform.position, lookAhead);
            var bestPath = GetBestPathForIndex(car.transform.position, paths, nearestIndex);

            var targetPosition = bestPath.Nodes[nearestIndex];

            Debug.DrawLine(car.transform.position, targetPosition, Color.red, 0.1f);

            var angleToTarget = (targetPosition - car.transform.position).normalized;

            var forwardAngle = Mathf.Atan2(car.transform.forward.z, car.transform.forward.x);

            var angle = AngleDir(car.transform.forward, angleToTarget, Vector3.up);
            var steerAngle = Mathf.Lerp(car.AiVariables.Angle, angle, 0.8f);
            car.AiVariables.Angle = steerAngle;

            var brake = Mathf.Abs(steerAngle) > 0.7f;

            car.UpdateInput(steerAngle:steerAngle, motor: brake ? 0.2f : UnityEngine.Random.Range(0.75f, 0.85f), brakes: 0f);

            if (car._timeNotGrounded > 5f)
                car.Reset();
            if (car.AiVariables.TimeWithVelocityBelowTreshold > 3f)
                car.Reset();
        }

        private TrackPath GetBestPathForIndex(Vector3 position, RaceTrackPaths paths, int index)
        {
            var pathsByDistance = paths.Paths.OrderByDescending(x => 
            {
                return Vector3.Distance(position, x.Nodes[index]);
            });

            return pathsByDistance.Skip(1).First();

            //for (int i = 0; i < paths.Paths.Length; i++)
            //{
            //    var node = paths.Paths[i].Nodes[index];

            //    var dst = Vector3.Distance(position, node);
            //    if (dst < distance)
            //    {
            //        path = paths.Paths[i];
            //        distance = dst;
            //    }
            //}
            //return path;
        }
        private float AngleDir(Vector3 forward, Vector3 targetDirection, Vector3 up)
        {
            Vector3 perp = Vector3.Cross(forward, targetDirection);
            float dir = Vector3.Dot(perp, up);

            if (dir > 0f)
            {
                return Mathf.Lerp(0f, 2f, Mathf.Clamp(dir, 0f, 1f));
            }
            else if (dir < 0f)
            {
                return Mathf.Lerp(-2f, 0f, Mathf.Clamp(dir, -1f, 0f));
            }
            else
            {
                return 0f;
            }
        }
    }
}
