using Assets._Project.Scripts.Tracks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;

namespace Assets._Project.Scripts.Editor
{
    [CustomEditor(typeof(RaceTrackNode))]
    public class RaceTrackNodeEditor : CustomEditorBase<RaceTrackNode>
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }

        public override void OnSceneGUI()
        {
            base.OnSceneGUI();
            var track = Target.Track;
            if (track == null) return;
            
        }
    }
}
