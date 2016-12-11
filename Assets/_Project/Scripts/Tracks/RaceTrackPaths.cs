using Assets._Project.Scripts.Paths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets._Project.Scripts.Tracks
{
    public class RaceTrackPaths : MonoBehaviour
    {
        public TrackPath[] Paths;
        public int Segments = 4;

        void Start()
        {
            var paths = new List<TrackPath>();
            for (int i = 0; i < Segments; i++)
                paths.Add(new TrackPath());

            int index = 0;
            Iterate((prevPoint, point, info) => 
            {
                paths[index].Nodes.Add(point);
                index++;
                if (index == Segments)
                    index = 0;
            });
            Paths = paths.ToArray();
        }

        private void Iterate(Action<Vector3, Vector3, IterableSplineInfo> callback)
        {
            var spline = GetComponent<CatmullRomSpline>();
            var track = GetComponent<RaceTrack>();

            spline.IterateSpline((info) =>
            {

                var position = info.Position;
                var rotation = info.Rotation;
                var upVector = Quaternion.FromToRotation(Vector3.up, info.UpVector).eulerAngles;

                var lastPosition = info.LastPosition;
                var lastRotation = info.LastRotation;
                var lastUpVector = Quaternion.FromToRotation(Vector3.up, info.LastUpVector).eulerAngles;

                var left = rotation * Vector3.left;
                var right = rotation * Vector3.right;

                var prevLeft = lastRotation * Vector3.left;
                var prevRight = lastRotation * Vector3.right;

                for (int i = 0; i < Segments; i++)
                {
                    float w = Mathf.Lerp(0.2f, 0.8f, (float)i / (float)(Segments - 1)) - 0.5f;
                    w *= track.TrackWidth;

                    var point = RaceTrackBuilder.RotatePointAroundPivot(position + (left * w), position, upVector);
                    var prevPoint = RaceTrackBuilder.RotatePointAroundPivot(lastPosition + (prevLeft * w), lastPosition, lastUpVector);

                    callback(prevPoint, point, info);
                }
            }, track.Resolution);
        }

        void OnDrawGizmos()
        {
            Iterate((prevPoint, point, info) =>
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawLine(prevPoint + info.LastUpVector, point + info.UpVector);
            });
        }
    }

    public class TrackPath
    {
        public List<Vector3> Nodes = new List<Vector3>();

        public int GetNearestIndex(Vector3 position, int lookAhead = 0)
        {
            var distance = float.MaxValue;
            int index = 0;

            for (int i = 0; i < Nodes.Count; i++)
            {
                var dst = Vector3.Distance(position, Nodes[i]);
                if(dst < distance)
                {
                    index = i;
                    distance = dst;
                }
            }

            // Return nearest + 1, so we always have one in front of us
            index += lookAhead;
            if(index > Nodes.Count - 1)
            {
                return index - Nodes.Count;
            }
            return index;
        }
    }
}
