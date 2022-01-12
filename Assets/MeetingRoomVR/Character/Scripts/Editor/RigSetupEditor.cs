using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Animations.Rigging;
using MeetingRoomVR.Character;

[CustomEditor(typeof(RigSetup))]
public class RigSetupEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();//DrawDefaultInspector();
        var rigSetup = (RigSetup)target;
        if (GUILayout.Button("Setup"))
        {
            rigSetup.TrySetup();
        }
    }
}
