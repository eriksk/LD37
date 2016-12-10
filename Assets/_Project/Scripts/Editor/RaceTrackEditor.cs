using Assets._Project.Scripts.Tracks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Assets._Project.Scripts.Editor
{
    [CustomEditor(typeof(RaceTrack))]
    public class RaceTrackEditor : CustomEditorBase<RaceTrack>
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Build"))
            {
                Target.Build();
            }
        }

        public override void OnSceneGUI()
        {
            base.OnSceneGUI();

            //var nodes = Target.Nodes;

            //if (nodes == null || nodes.Count < 4) return;

            //float segmentSize = 1f;

            //var getNode = new Func<int, Vector3>((index) => 
            //{
            //    if (index > nodes.Count - 4)
            //        index = index - nodes.Count;
            //    return nodes[index].transform.position;
            //};

            //for (int i = 0; i < nodes.Count; i++)
            //{
            //    var n1 = getNode(i);
            //    var n2 = getNode(i + 1);
            //    var n3 = getNode(i + 2);
            //    var n4 = getNode(i + 3);

            //    //EditorGUI.BeginChangeCheck();
            //    //var handle = Handles.DoPositionHandle(node.transform.position, node.transform.rotation);
            //    //if (EditorGUI.EndChangeCheck())
            //    //{
            //    //    Undo.RecordObject(node.transform, "Move Point");
            //    //    EditorUtility.SetDirty(node.transform);
            //    //    node.transform.position = handle;
            //    //}

            //    //var distance = Vector3.Distance(node.transform.position, next.transform.position);

            //    //var segments = Mathf.Max(1, distance / segmentSize);

            //    for (int j = 0; j < segments; j++)
            //    {
                    
            //        Handles.color = Color.yellow;
            //        Handles.DrawLine(node.transform.position, next.transform.position);
            //    }
            //}
        }
    }
}
