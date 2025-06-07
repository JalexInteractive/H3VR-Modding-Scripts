using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Packer
{
    [CustomEditor(typeof(Door))]
    public class DoorEditor : Editor
    {

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUILayout.Space(10);

            var script = (Door)target;

            if (GUILayout.Button("Set Close Offset", GUILayout.Height(20)))
            {
                script.SetCloseOffset();
            }

            if (GUILayout.Button("Set Open Offset", GUILayout.Height(20)))
            {
                script.SetOpenOffset();
            }

            GUILayout.Space(7);

            if (GUILayout.Button("Move To Close Offset", GUILayout.Height(20)))
            {
                script.MoveToCloseOffset();
            }

            if (GUILayout.Button("Move To Open Offset", GUILayout.Height(20)))
            {
                script.MoveToOpenOffset();
            }

            GUILayout.Space(20);

            if (GUILayout.Button("Set Close Rotation", GUILayout.Height(20)))
            {
                script.SetCloseRotation();
            }
            
            if (GUILayout.Button("Set Open Rotation", GUILayout.Height(20)))
            {
                script.SetOpenRotation();
            }

            GUILayout.Space(7);

            if (GUILayout.Button("Move To Close Rotation", GUILayout.Height(20)))
            {
                script.MoveToCloseRotation();
            }

            if (GUILayout.Button("Move To Open Rotation", GUILayout.Height(20)))
            {
                script.MoveToOpenRotation();
            }
        }
    }
}