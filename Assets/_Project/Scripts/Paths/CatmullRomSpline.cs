using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets._Project.Scripts.Paths
{
    public class CatmullRomSpline : MonoBehaviour
    {
        //Has to be at least 4 points
        public List<Vector3> controlPointsList
        {
            get
            {
                var children = new List<Vector3>();
                for (int i = 0; i < transform.childCount; i++)
                    children.Add(transform.GetChild(i).position);
                return children;
            }
        }
        //Are we making a line or a loop?
        public bool isLooping = true;

        //Display without having to press play
        void OnDrawGizmos()
        {
            Gizmos.color = Color.white;

            if (controlPointsList == null) return;
            //Draw the Catmull-Rom spline between the points
            for (int i = 0; i < controlPointsList.Count; i++)
            {
                //Cant draw between the endpoints
                //Neither do we need to draw from the second to the last endpoint
                //...if we are not making a looping line
                if ((i == 0 || i == controlPointsList.Count - 2 || i == controlPointsList.Count - 1) && !isLooping)
                {
                    continue;
                }

                DisplayCatmullRomSpline(i);
            }
        }

        public void AddNode()
        {
            var node = new GameObject("node");
            node.transform.parent = transform;
            node.transform.localPosition = new Vector3(1, 0, 1);
        }


        public void IterateSpline(Action<Vector3, Quaternion, Vector3, Quaternion> callback, float resolution)
        {

            var nodes = new List<SplineNodeInfo>();

            for (int i = 0; i < controlPointsList.Count; i++)
            {
                //Cant draw between the endpoints
                //Neither do we need to draw from the second to the last endpoint
                //...if we are not making a looping line
                if ((i == 0 || i == controlPointsList.Count - 2 || i == controlPointsList.Count - 1) && !isLooping)
                {
                    continue;
                }

                var pos = i;

                //The 4 points we need to form a spline between p1 and p2
                Vector3 p0 = controlPointsList[ClampListPos(pos - 1)];
                Vector3 p1 = controlPointsList[pos];
                Vector3 p2 = controlPointsList[ClampListPos(pos + 1)];
                Vector3 p3 = controlPointsList[ClampListPos(pos + 2)];

                //The start position of the line
                Vector3 lastPos = p1;

                //How many times should we loop?
                int loops = Mathf.FloorToInt(1f / resolution);

                for (int j = 1; j <= loops; j++)
                {
                    //Which t position are we at?
                    float t = j * resolution;

                    //Find the coordinate between the end points with a Catmull-Rom spline
                    Vector3 newPos = GetCatmullRomPosition(t, p0, p1, p2, p3);

                    nodes.Add(new SplineNodeInfo()
                    {
                        Position = newPos
                    });

                    //Save this pos so we can draw the next line segment
                    lastPos = newPos;
                }
            }

            for (int i = 0; i < nodes.Count; i++)
            {
                var node = nodes[i];
                var next = nodes[i == nodes.Count - 1 ? 0 : i + 1];
                node.Rotation = Quaternion.LookRotation((node.Position - next.Position).normalized);
            }

            for (int i = 0; i < nodes.Count; i++)
            {
                var node = nodes[i];
                var next = nodes[i == nodes.Count - 1 ? 0 : i + 1];

                callback(node.Position, node.Rotation, next.Position, next.Rotation);
            }
        }

        //Display a spline between 2 points derived with the Catmull-Rom spline algorithm
        void DisplayCatmullRomSpline(int pos)
        {
            //The 4 points we need to form a spline between p1 and p2
            Vector3 p0 = controlPointsList[ClampListPos(pos - 1)];
            Vector3 p1 = controlPointsList[pos];
            Vector3 p2 = controlPointsList[ClampListPos(pos + 1)];
            Vector3 p3 = controlPointsList[ClampListPos(pos + 2)];

            //The start position of the line
            Vector3 lastPos = p1;

            //The spline's resolution
            //Make sure it's is adding up to 1, so 0.3 will give a gap, but 0.2 will work
            float resolution = 0.2f;

            //How many times should we loop?
            int loops = Mathf.FloorToInt(1f / resolution);

            for (int i = 1; i <= loops; i++)
            {
                //Which t position are we at?
                float t = i * resolution;

                //Find the coordinate between the end points with a Catmull-Rom spline
                Vector3 newPos = GetCatmullRomPosition(t, p0, p1, p2, p3);

                //Draw this line segment
                Gizmos.color = Color.green;
                Gizmos.DrawLine(lastPos, newPos);

                //Save this pos so we can draw the next line segment
                lastPos = newPos;
            }
        }

        //Clamp the list positions to allow looping
        int ClampListPos(int pos)
        {
            if (pos < 0)
            {
                pos = controlPointsList.Count - 1;
            }

            if (pos > controlPointsList.Count)
            {
                pos = 1;
            }
            else if (pos > controlPointsList.Count - 1)
            {
                pos = 0;
            }

            return pos;
        }

        //Returns a position between 4 Vector3 with Catmull-Rom spline algorithm
        //http://www.iquilezles.org/www/articles/minispline/minispline.htm
        Vector3 GetCatmullRomPosition(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
        {
            //The coefficients of the cubic polynomial (except the 0.5f * which I added later for performance)
            Vector3 a = 2f * p1;
            Vector3 b = p2 - p0;
            Vector3 c = 2f * p0 - 5f * p1 + 4f * p2 - p3;
            Vector3 d = -p0 + 3f * p1 - 3f * p2 + p3;

            //The cubic polynomial: a + b * t + c * t^2 + d * t^3
            Vector3 pos = 0.5f * (a + (b * t) + (c * t * t) + (d * t * t * t));

            return pos;
        }
    }

    public class SplineNodeInfo
    {
        public Vector3 Position;
        public Quaternion Rotation;
    }
}
