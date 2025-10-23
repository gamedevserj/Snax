using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace UISystem.EditorScripts
{
    [CustomEditor(typeof(GridController), true)]
    public class GridControllerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            var controller = (GridController)target;
            if (GUILayout.Button("Create grid"))
            {
                controller.CreateGrid();
            }
            if (controller.HasGrid && GUILayout.Button("Clear grid"))
            {
                controller.ClearGrid();
            } 
        }
    }
}