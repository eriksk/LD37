using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets._Project.Scripts.Tracks
{
    public class RaceTrackNode : MonoBehaviour
    {
        public RaceTrack Track
        {
            get
            {
                if (transform.parent == null) return null;
                return transform.parent.GetComponent<RaceTrack>();
            }
        }
    }
}
