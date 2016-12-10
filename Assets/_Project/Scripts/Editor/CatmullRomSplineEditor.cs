using Assets._Project.Scripts.Paths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Assets._Project.Scripts.Editor
{
    [CustomEditor(typeof(CatmullRomSpline))]
    public class CatmullRomSplineEditor : CustomEditorBase<CatmullRomSpline>
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if(GUILayout.Button("Add Node"))
            {
                Target.AddNode();
            }
        }
    }
}
