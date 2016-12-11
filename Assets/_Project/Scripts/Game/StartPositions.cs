using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets._Project.Scripts.Game
{
    public class StartPositions : MonoBehaviour
    {
        public StartPosition[] Positions
        {
            get
            {
                var children = new List<Transform>();
                for (int i = 0; i < transform.childCount; i++)
                    children.Add(transform.GetChild(i));

                return children.Select(x => new StartPosition()
                {
                    Position = x.position,
                    Rotation = Quaternion.LookRotation(transform.forward)
                }).ToArray();
            }
        }
    }

    [Serializable]
    public class StartPosition
    {
        public Vector3 Position;
        public Quaternion Rotation;
    }
}
