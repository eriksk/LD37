using Assets._Project.Scripts.Paths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Assets._Project.Scripts.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(CatmullRomSplineNode))]
    public class CatmullRomSplineNodeEditor : CustomEditorBase<CatmullRomSplineNode>
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (Targets == null)
                return;

            if (GUILayout.Button("LookStraightUp"))
            {
                foreach (var t in Targets)
                    t.transform.rotation = Quaternion.identity;
            }

            if (GUILayout.Button("Look at next"))
            {
                foreach (var t in Targets)
                {
                    var next = t.NextNode;
                    if(next != null)
                        t.transform.rotation = Quaternion.LookRotation(next.transform.position - t.transform.position);
                }
            }
        }
    }
}
