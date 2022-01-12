using MeetingRoomVR.Character.Infrastructure;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

namespace MeetingRoomVR.Character
{
    [RequireComponent(typeof(CharacterController))]
    public class MultiplatformPlayerController : MonoBehaviour
    {
        public float HeadTetherDistance = 1.8f;
        [SerializeField] private float WalkSpeed = 1.2f;
        [SerializeField] private float SprintSpeed = 3f;
        [SerializeField] private ControlsType controlsOnStartup;
        [SerializeField] private Transform vrHeadToFollow;
        public float CurrentSpeed => controls.IsSprinting ? SprintSpeed : WalkSpeed;

        private IControls controls;
        #region HashedComponents
        [SerializeField] private Transform currentHead;
        [SerializeField] private Camera playerCamera;
        private Transform cameraTransform;
        private CharacterController characterController;
        private Transform transform;
        private AnimatedAvatar avatar;
        private PCHeadController pcHeadController;
        private Player steamPlayer;
        #endregion HashedComponents

        private Vector3 position => transform.position;
        private Vector3 headPosition => currentHead.position;

        public Vector3 FacingDirection
        {
            get
            {
                var headForwardProjection = Vector3.ProjectOnPlane(currentHead.forward, transform.up).normalized;
                return Vector3.Dot(transform.up, currentHead.up) < 0
                    ? -headForwardProjection
                    : headForwardProjection;
            }
        }

        IEnumerator LateCall(Action method)
        {
            yield return new WaitForSeconds(2);
            method();
        }

        void Awake()
        {
            pcHeadController = GetComponentInChildren<PCHeadController>();
            characterController = GetComponent<CharacterController>();
            transform = GetComponent<Transform>();
            avatar = GetComponentInChildren<AnimatedAvatar>();
            steamPlayer = GetComponentInChildren<Player>();
            if (pcHeadController is null)
                throw new UnityException("PCHeadController component was not found in Player");
            cameraTransform = playerCamera.GetComponent<Transform>();
        }

        void Start()
        {
            SetupControlsPC();
            //StartCoroutine(LateCall(SetupControlsVR));
            print(TrySetControls(controlsOnStartup) ? "Controls successfully set" : "Couldn't set controls. Reverting to PC Controls");
        }

        private void FixedUpdate()
        {
            ConstrainHeadTether();
        }
        void Update()
        {
            //print(avatar.Head.Transform.position);
            if (Input.GetKeyDown(KeyCode.P))
                print("Setting PC Controls: " + TrySetControls(ControlsType.PC));
            if (Input.GetKeyDown(KeyCode.V))
                print("Setting VR Controls: " + TrySetControls(ControlsType.VR));
            //if (Input.GetKeyDown(KeyCode.Q))
            //    avatar.Head.StartFollowing(currentHead);
            //if (Input.GetKeyDown(KeyCode.E))
            //    avatar.Head.StopFollowing();
            WalkInDirection(controls.MovementVector);
        }

        public bool TrySetControls(ControlsType controlsType)
        {
            switch (controlsType)
            {
                case ControlsType.PC:
                    SetupControlsPC();
                    return true;
                case ControlsType.VR:
                    //if (SteamVR.instance == null && vrHeadToFollow != null)
                    //{
                    //    SetupControlsPC();
                    //    return false;
                    //}
                    SetupControlsVR();
                    return true;
                default:
                    throw new NotImplementedException();
            }
        }

        private void ConstrainHeadTether()
        {
            var headOffsetFromBodyRoot = headPosition - position;
            if (headOffsetFromBodyRoot.sqrMagnitude > HeadTetherDistance * HeadTetherDistance)
            {
                currentHead.SetParent(null, true);
                transform.position = new Vector3(
                    headPosition.x, 
                    headPosition.y - Mathf.Abs(Mathf.Min(avatar.HeadLocalHeight, avatar.StandingHeadHeight)),
                    headPosition.z);
                //резкая телепортация, заменить на плавную/передвижение в точку
                currentHead.SetParent(transform, true);
            }
        }

        private void WalkInDirection(Vector2 inputMoveVector)
        {
            inputMoveVector = Vector3.ClampMagnitude(inputMoveVector, 1);
            var forward = FacingDirection;
            //var lookDirection = Vector3.ProjectOnPlane(head.forward, transform.up).normalized;//avatar.HeadForwardProjection.normalized;//
            //var forward = Vector3.Dot(avatar.transform.forward, head.forward) < 0
            //    ? -lookDirection
            //    : lookDirection;
            var right = Vector3.Cross(transform.up, forward);
            var worldSpaceMoveVector = Vector3.ClampMagnitude(
                forward * inputMoveVector.y + right * inputMoveVector.x,
                1) * CurrentSpeed;
            characterController.SimpleMove(worldSpaceMoveVector);
            //var velocity = new Vector2(characterController.velocity.x, characterController.velocity.z);
            avatar.SetMovementVector(inputMoveVector * CurrentSpeed);
        }

        private void SetupControlsPC()
        {
            steamPlayer.enabled = false;
            controls = new PCControls();
            pcHeadController.enabled = true;
            pcHeadController.StandingHeight = avatar.StandingHeadHeight;
            pcHeadController.CrouchingHeight = avatar.CrouchingHeadHeight;
            currentHead = pcHeadController.transform;
            cameraTransform.SetParent(currentHead, false);
            avatar.Head.StartFollowing(currentHead);
            avatar.LeftHand.StopFollowing();
            avatar.RightHand.StopFollowing();
        }

        private void SetupControlsVR()
        {
            steamPlayer.enabled = true;
            controls = new VRControls();
            pcHeadController.enabled = false;
            currentHead = vrHeadToFollow;
            cameraTransform.SetParent(currentHead, false);
            avatar.Head.StartFollowing(currentHead);
            //avatar.LeftHand.StartFollowing(steamPlayer.leftHand.transform);
            //avatar.RightHand.StartFollowing(steamPlayer.rightHand.transform);
        }

        private void OnDrawGizmos()
        {
            //Gizmos.DrawLine(Vector3.zero, HeadForwardProjection);
            if (currentHead != null)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(transform.position, transform.position + FacingDirection);
            }
        }
    }
}
