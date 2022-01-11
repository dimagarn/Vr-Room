using MeetingRoomVR.Character;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AnimatedAvatar))]
public class AvatarEditor : Editor
{
    private bool updateSettingsInEditor;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (Application.isPlaying)
        {
            var avatar = (AnimatedAvatar)target;
            if (updateSettingsInEditor)
                avatar.RefreshStartupSettings();
        }
        if (GUILayout.Button("Enable Editor updates"))
            updateSettingsInEditor = true;
        if (GUILayout.Button("Disable Editor updates"))
            updateSettingsInEditor = false;
    }
}
