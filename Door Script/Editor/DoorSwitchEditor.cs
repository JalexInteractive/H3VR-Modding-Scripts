using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Packer
{
    [CustomEditor(typeof(DoorSwitch))]
    public class DoorSwitchEditor : Editor
    {

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUILayout.Space(5);

            var script = (DoorSwitch)target;

            if (GUILayout.Button("Push the button", GUILayout.Height(20)))
            {
                if (Application.isPlaying)
                    script.ToggleDoors();
            }

            GUILayout.Space(5);

        }
    }
}