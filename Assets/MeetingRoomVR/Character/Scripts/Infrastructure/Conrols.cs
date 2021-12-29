using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MeetingRoomVR.Character.Infrastructure
{
    public enum ControlsType
    {
        PC,
        VR
    }

    public interface IControls
    {
        public Vector2 MovementVector { get; }
        public bool IsSprinting { get; }
    }

    public class PCControls : IControls
    {
        public Vector2 MovementVector
        {
            get 
            {
                var rawInputVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
                return Vector3.ClampMagnitude(rawInputVector, 1); 
            }
        }

        public bool IsSprinting => Input.GetKey(KeyCode.LeftShift);
    }

    public class VRControls : IControls
    {
        public Vector2 MovementVector => Vector3.ClampMagnitude(
            new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")),
            1);

        public bool IsSprinting => Input.GetKey(KeyCode.LeftShift);
    }
}