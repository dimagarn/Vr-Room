using MeetingRoomVR.Character.Infrastructure;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class UniversalPlayer : Player
{
    public float HeadTetherDistance = 1.8f;
    [SerializeField] private float WalkSpeed = 1.2f;
    [SerializeField] private float SprintSpeed = 3f;

    public float SpeedMultiplier => controls.IsSprinting ? SprintSpeed : WalkSpeed;
    public bool VRIsAvailable => SteamVR.instance != null;
    private IControls controls;
}
