using MeetingRoomVR.Character;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AnimatedAvatar))]
public class AvatarEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (Application.isPlaying)
        {
            var avatar = (AnimatedAvatar)target;
            avatar.RefreshEditorSettings();
        }
    }
}
