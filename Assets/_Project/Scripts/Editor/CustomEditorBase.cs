using System;
using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

namespace Assets._Project.Scripts.Editor
{
    public class CustomEditorBase<T> : UnityEditor.Editor where T : class
    {
        public T Target
        {
            get { return target as T; }
        }
        public T[] Targets
        {
            get
            {
                if(targets == null)
                    return new T[0];

                var ts = new List<T>();
                foreach (var target in targets)
                    ts.Add(target as T);
                return ts.ToArray();
            }
        }

        protected bool TrigVector2Field(string name, Func<T, Vector2> callback, out Vector2 newValue)
        {
            var oldValue = callback(Target);
            var nValue = EditorGUILayout.Vector2Field(name, oldValue);
            newValue = nValue;
            return oldValue == newValue;
        }

        protected bool TrigColorField(string name, Func<T, Color> callback, out Color newValue)
        {
            var oldValue = callback(Target);
            var nValue = EditorGUILayout.ColorField(name, oldValue);
            newValue = nValue;

            const float tolerance = 0.00001f;

            if (Math.Abs(oldValue.r - newValue.r) > tolerance)
                return true;
            if (Math.Abs(oldValue.g - newValue.g) > tolerance)
                return true;
            if (Math.Abs(oldValue.b - newValue.b) > tolerance)
                return true;
            if (Math.Abs(oldValue.a - newValue.a) > tolerance)
                return true;

            return false;
        }

        protected bool TrigIntField(string name, Func<T, int> callback, out int newValue)
        {
            var oldValue = callback(Target);
            var nValue = EditorGUILayout.IntField(name, oldValue);
            newValue = nValue;
            return oldValue == newValue;
        }

        protected bool TrigFloatField(string name, Func<T, float> callback, out float newValue)
        {
            var oldValue = callback(Target);
            var nValue = EditorGUILayout.FloatField(name, oldValue);
            newValue = nValue;
            return Math.Abs(oldValue - newValue) < 0.000001f;
        }

        protected bool TrigTextAreaField(string name, Func<T, string> callback, out string newValue)
        {
            var oldValue = callback(Target);
            var nValue = EditorGUILayout.TextArea(oldValue);
            newValue = nValue;
            return oldValue != newValue;
        }

        public virtual void OnSceneGUI()
        {
        }

        protected int ControlId
        {
            get { return GUIUtility.GetControlID(FocusType.Passive); }
        }
    }
}