using Assets._Project.Scripts.Meshes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets._Project.Scripts.Tracks
{
    public class RaceTrackBuilder
    {
        public static MeshData Build(Material material, RaceTrack track, float resolution, float thickness)
        {
            var data = new MeshData();

            int index = 0;

            track.Track.IterateSpline((lastPosition, lastRotation, position, rotation) =>
            {
                var left = rotation * Vector3.left;
                var right = rotation * Vector3.right;

                var prevLeft = lastRotation * Vector3.left;
                var prevRight = lastRotation * Vector3.right;


                var leftPoint = position + (left * track.TrackWidth * 0.5f);
                var rightPoint = position + (left * -track.TrackWidth * 0.5f);

                var prevLeftPoint = lastPosition + (prevLeft * track.TrackWidth * 0.5f);
                var prevRightPoint = lastPosition + (prevLeft * -track.TrackWidth * 0.5f);

                data.Vertices.Add(prevLeftPoint);
                data.Vertices.Add(prevRightPoint);
                data.Vertices.Add(leftPoint);
                data.Vertices.Add(rightPoint);

                // top

                data.Indices.Add(index + 0);
                data.Indices.Add(index + 1);
                data.Indices.Add(index + 2);
                data.Indices.Add(index + 1);
                data.Indices.Add(index + 3);
                data.Indices.Add(index + 2);

                data.Uvs.AddRange(new Vector2[] 
                {
                    new Vector2(0, 0),
                    new Vector2(1, 0),
                    new Vector2(0, 1),
                    new Vector2(1, 1),
                });

                var down = new Vector3(0f, -thickness, 0f);

                data.Vertices.Add(prevLeftPoint + down);
                data.Vertices.Add(prevRightPoint + down);
                data.Vertices.Add(leftPoint + down);
                data.Vertices.Add(rightPoint + down);

                // bottom
                data.Indices.Add(index + 4);
                data.Indices.Add(index + 6);
                data.Indices.Add(index + 5);
                data.Indices.Add(index + 5);
                data.Indices.Add(index + 6);
                data.Indices.Add(index + 7);

                // left
                data.Indices.Add(index + 6);
                data.Indices.Add(index + 4);
                data.Indices.Add(index + 2);
                data.Indices.Add(index + 4);
                data.Indices.Add(index + 0);
                data.Indices.Add(index + 2);

                // right
                data.Indices.Add(index + 5);
                data.Indices.Add(index + 7);
                data.Indices.Add(index + 1);
                data.Indices.Add(index + 7);
                data.Indices.Add(index + 3);
                data.Indices.Add(index + 1);

                data.Uvs.AddRange(new Vector2[]
                {
                    new Vector2(0, 0),
                    new Vector2(1, 0),
                    new Vector2(0, 1),
                    new Vector2(1, 1),
                });

                index += 8;
            }, resolution);

            return data;
        }
    }
}
