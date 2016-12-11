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
        public static TrackMeshes Build(Material material, RaceTrack track, float resolution, float thickness, float sideThickness)
        {
            var data = new MeshData();
            var sidesData = new MeshData();

            int index = 0;
            int index2 = 0;

            track.Track.IterateSpline((splineNodeInfo) =>
            {
                var position = splineNodeInfo.Position;
                var rotation = splineNodeInfo.Rotation;
                var upVector = Quaternion.FromToRotation(Vector3.up, splineNodeInfo.UpVector).eulerAngles;

                var lastPosition = splineNodeInfo.LastPosition;
                var lastRotation = splineNodeInfo.LastRotation;
                var lastUpVector = Quaternion.FromToRotation(Vector3.up, splineNodeInfo.LastUpVector).eulerAngles;

                var left =  rotation * Vector3.left;
                var right = rotation * Vector3.right;

                var prevLeft = lastRotation * Vector3.left;
                var prevRight = lastRotation * Vector3.right;

                var leftPoint = RotatePointAroundPivot(position + (left * track.TrackWidth * 0.5f), position, upVector);
                var rightPoint = RotatePointAroundPivot(position + (right * track.TrackWidth * 0.5f), position, upVector);

                var prevLeftPoint = RotatePointAroundPivot(lastPosition + (prevLeft * track.TrackWidth * 0.5f), lastPosition, lastUpVector);
                var prevRightPoint = RotatePointAroundPivot(lastPosition + (prevRight * track.TrackWidth * 0.5f), lastPosition, lastUpVector);

                var prevMiddlePoint = lastPosition;
                var middlePoint = position;

                index += AddQuad(data, index,
                    prevLeftPoint,
                    prevMiddlePoint,
                    leftPoint,
                    middlePoint,
                    new Vector2[]
                    {
                        new Vector2(0, 0),
                        new Vector2(0.5f, 0),
                        new Vector2(0, 0.5f),
                        new Vector2(0.5f, 0.5f),
                    }
                );
                index += AddQuad(data, index,
                    prevMiddlePoint,
                    prevRightPoint,
                    middlePoint,
                    rightPoint,
                    new Vector2[]
                    {
                        new Vector2(0, 0),
                        new Vector2(0.5f, 0),
                        new Vector2(0, 0.5f),
                        new Vector2(0.5f, 0.5f),
                    }
                );

                var prevSideBoxHeight = lastRotation * new Vector3(0f, sideThickness* 0.5f, 0f);
                var sideBoxHeight = rotation * new Vector3(0f, sideThickness * 0.5f, 0f);

                var prevNarrowCornerOffsetLeft = lastRotation * new Vector3(-sideThickness, -thickness, 0f);
                var prevNarrowCornerOffsetRight = lastRotation * new Vector3(sideThickness, -thickness, 0f);

                var narrowCornerOffsetLeft = rotation * new Vector3(-sideThickness, -thickness, 0f);
                var narrowCornerOffsetRight = rotation * new Vector3(sideThickness, -thickness, 0f);

                // left direct border
                index2 += AddQuad(sidesData, index2,
                    prevLeftPoint,
                    leftPoint,
                    prevLeftPoint + prevSideBoxHeight,
                    leftPoint + sideBoxHeight
                    );

                // right direct border
                index2 += AddQuad(sidesData, index2,
                    rightPoint,
                    prevRightPoint,
                    rightPoint + sideBoxHeight,
                    prevRightPoint + prevSideBoxHeight
                    );

                // narrow corner down left
                index2 += AddQuad(sidesData, index2,
                    prevLeftPoint + prevSideBoxHeight,
                    leftPoint + sideBoxHeight,
                    prevLeftPoint + prevNarrowCornerOffsetLeft,
                    leftPoint + narrowCornerOffsetLeft
                    );

                // narrow corner down left
                index2 += AddQuad(sidesData, index2,
                    rightPoint + sideBoxHeight,
                    prevRightPoint + prevSideBoxHeight,
                    rightPoint + narrowCornerOffsetRight,
                    prevRightPoint + prevNarrowCornerOffsetRight
                    );

                // bottom
                index2 += AddQuad(sidesData, index2,
                    leftPoint + narrowCornerOffsetLeft,
                    rightPoint + narrowCornerOffsetRight,
                    prevLeftPoint + prevNarrowCornerOffsetLeft,
                    prevRightPoint + prevNarrowCornerOffsetRight
                    );

            }, resolution);

            return new TrackMeshes()
            {
                Track = data,
                Sides = sidesData
            };
        }

        public static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
        {
            Vector3 dir = point - pivot; // get point direction relative to pivot
            dir = Quaternion.Euler(angles) * dir; // rotate it
            point = dir + pivot; // calculate rotated point
            return point; // return it
        }

        private static int AddQuad(MeshData data, int index, Vector3 bottomLeft, Vector3 bottomRight, Vector3 topLeft, Vector3 topRight,
            Vector2[] uvs = null)
        {
            data.Vertices.Add(bottomLeft);
            data.Vertices.Add(bottomRight);
            data.Vertices.Add(topLeft);
            data.Vertices.Add(topRight);

            data.Indices.Add(index + 0);
            data.Indices.Add(index + 1);
            data.Indices.Add(index + 2);
            data.Indices.Add(index + 1);
            data.Indices.Add(index + 3);
            data.Indices.Add(index + 2);

            if (uvs == null)
            {
                data.Uvs.AddRange(new[]
                {
                    new Vector2(),
                    new Vector2(),
                    new Vector2(),
                    new Vector2()
                });
            }
            else
            {
                data.Uvs.AddRange(uvs);
            }

            return 4;
        }
    }

    public class TrackMeshes
    {
        public MeshData Track;
        public MeshData Sides;
    }
}
