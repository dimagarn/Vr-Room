using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MeetingRoomVR.Character
{
    public class PCHeadController : MonoBehaviour
    {
        public float Sensitivity = 0.5f;
        public bool HoldToCrouch;
        public float StandingHeight { get; set; }
        public float CrouchingHeight { get; set; }

        private Transform transform;
        private Vector2 viewRotation;
        private bool isCrouching = false;

        private const string mouseX = "Mouse X";
        private const string mouseY = "Mouse Y";
        private Vector2 mouseInput => new Vector2(Input.GetAxis(mouseX), Input.GetAxis(mouseY));
        private bool isCrouchKeyBeingHold => Input.GetKey(KeyCode.C);
        private bool isCrouchKeyPressed => Input.GetKeyDown(KeyCode.C);

        void Awake()
        {
            transform = base.transform;
        }

        void Update()
        {

            isCrouching = HoldToCrouch
                ? isCrouchKeyBeingHold
                : isCrouchKeyPressed
                ? !isCrouching
                : isCrouching;
            var headHeight = (isCrouching ? CrouchingHeight : StandingHeight);
            transform.localPosition = new Vector3(
                transform.localPosition.x,
                Mathf.Lerp(transform.localPosition.y, headHeight, Time.deltaTime * 3),
                transform.localPosition.z);
            viewRotation += mouseInput * Sensitivity;
            transform.localEulerAngles = new Vector3(-viewRotation.y, viewRotation.x, 0);
        }
    }
}