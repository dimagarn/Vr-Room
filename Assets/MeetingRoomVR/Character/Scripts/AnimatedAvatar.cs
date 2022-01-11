using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using System.Linq;
using UnityEngine.Animations;
using MeetingRoomVR.Character.Infrastructure;
using System;

namespace MeetingRoomVR.Character
{
    public class AnimatedAvatar : MonoBehaviour
    {
        #region EditorSettings
        [Space(), Header("Updates only in Editor")]
        public bool HideHead;
        public bool HideHands;
        public Transform HeadToFollow;
        public Transform LeftHandToFollow;
        public Transform RightHandToFollow;
        public Vector3 LeftHandRotation;
        public Vector3 RightHandRotation;
        [Space(), Header("Runtime check")]
        public bool UseHeadTethering = true;
        public float HeadTetherDistance = 1.8f;
        #endregion EditorSettings
        public float StandingHeadHeight { get; private set; } = 1.7f;
        public float CrouchingHeadHeight { get; private set; } = 1.2f;
        public TrackingIK Head { get; private set; }
        public TrackingIK LeftHand { get; private set; }
        public TrackingIK RightHand { get; private set; }
        public Transform transform { get; private set; }
        private Animator animator;
        private Animator cloneAnimator;
        private MultiPositionConstraint hipsPositionConstraint;
        private SkinnedMeshRenderer headRenderer;
        private SkinnedMeshRenderer handsRenderer;
        private Vector3 vectorRightToReach;

        #region HashedStrings
        private readonly int crouchFloatHash = Animator.StringToHash("Crouch");
        private readonly int horizontalFloatHash = Animator.StringToHash("Horizontal");
        private readonly int verticalFloatHash = Animator.StringToHash("Vertical");
        #endregion

        public Vector3 HeadForwardProjection => Vector3.ProjectOnPlane(Head.Transform.forward, transform.up);

        public Vector3 HeadFacingDirection => Vector3.Dot(transform.forward, Head.Transform.forward) < 0
                ? -HeadForwardProjection.normalized
                : HeadForwardProjection.normalized;
        public float HeadLocalHeight
            => Head.Transform.position.y - transform.position.y;

        private float crouchValue => Mathf.InverseLerp(
            StandingHeadHeight, 
            CrouchingHeadHeight, 
            HeadLocalHeight);

        private bool bodyNeedsToTurn => Vector3.Dot(Head.Transform.right, transform.right) < 0;

        void Awake()
        {
            transform = base.transform;
            var animators = transform.GetComponentsInChildren<Animator>();
            if (animators.Length != 2)
                throw new UnityException("Avatar must have only 2 Animators: with rig and without one");
            foreach (var animComponent in animators)
            {
                if (animComponent.TryGetComponent<RigBuilder>(out var rigBuilder))
                    animator = animComponent;
                else
                    cloneAnimator = animComponent;
            }
            cloneAnimator.runtimeAnimatorController = animator.runtimeAnimatorController;
            foreach (var meshRenderer in animator.GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                switch (meshRenderer.name)
                {
                    case "Head":
                        headRenderer = meshRenderer;
                        break;
                    case "Hands":
                        handsRenderer = meshRenderer;
                        break;
                }
            }
            foreach (var constraintComponent in animator.GetComponentsInChildren<ParentConstraint>()) //Создаем TrackingIK объекты
            {
                switch (constraintComponent.name)
                {
                    case TrackingIK.HeadIKName:
                        Head = new TrackingIK(constraintComponent.transform);
                        var hipsConstraints = constraintComponent.GetComponentsInChildren<MultiPositionConstraint>();
                        if (hipsConstraints.First() is null || hipsConstraints.Length > 1)
                            throw new UnityException($"Couldn't find proper constraint for hips in {constraintComponent.name}");
                        hipsPositionConstraint = hipsConstraints.First();//Single()
                        break;
                    case TrackingIK.LeftHandIKName:
                        LeftHand = new TrackingIK(constraintComponent.transform);
                        break;
                    case TrackingIK.RightHandIKName:
                        RightHand = new TrackingIK(constraintComponent.transform);
                        break;
                    default:
                        throw new NotSupportedException();
                }
            }
        }

        void Start()
        {
            Head.StopFollowing();
            RightHand.StopFollowing();
            LeftHand.StopFollowing();
            RefreshStartupSettings();
        }

        public void RefreshStartupSettings()
        {
            Head.StartFollowing(HeadToFollow);
            RightHand.StartFollowing(RightHandToFollow);
            LeftHand.StartFollowing(LeftHandToFollow);
            LeftHand.RotationOffset = LeftHandRotation;
            RightHand.RotationOffset = RightHandRotation;
            headRenderer.enabled = !HideHead;
            handsRenderer.enabled = !HideHands;
        }

        private void SetAnimatorFloat(int hashedId, float value)
        {
            animator.SetFloat(hashedId, value);
            cloneAnimator.SetFloat(hashedId, value);
        }

        private void SetCrouchState(float crouchValue)
        {
            SetAnimatorFloat(crouchFloatHash, crouchValue);
            var hipsZOffset = Mathf.LerpUnclamped(0, -0.23f, Vector3.Dot(transform.up, Head.Transform.forward) * (crouchValue * 0.5f + 1));
            hipsPositionConstraint.data.offset = new Vector3(0, hipsZOffset, -0.59f);
        }
        private void ConstrainHeadTether()
        {
            //резкая телепортация, заменить на плавную/передвижение в точку
            var headOffsetFromBodyRoot = Head.Transform.position - transform.position;
            if (headOffsetFromBodyRoot.sqrMagnitude > HeadTetherDistance * HeadTetherDistance)
            {
                var headPosition = Head.TargetingTransform.position;
                transform.position = new Vector3(
                    headPosition.x,
                    transform.position.y,
                    headPosition.z);
                Head.TargetingTransform.position = headPosition;
            }
        }

        [SynchronizeMe]
        public void SetMovementVector(Vector2 vector)
        {
            SetAnimatorFloat(horizontalFloatHash, vector.x);
            SetAnimatorFloat(verticalFloatHash, vector.y);
            if (vector != Vector2.zero)
                FaceDirection(HeadFacingDirection);
        }

        private void FaceDirection(Vector3 direction)
        {
            var vectorRight = Vector3.Cross(transform.up, direction);
            vectorRightToReach = vectorRight;
        }

        void Update()
        {
            SetCrouchState(crouchValue);
            if (bodyNeedsToTurn)
                vectorRightToReach = Vector3.ProjectOnPlane(Head.Transform.right, transform.up);
            transform.right = Vector3.Lerp(transform.right, vectorRightToReach, Time.deltaTime * 5);
            if (UseHeadTethering)
                ConstrainHeadTether();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            if (transform != null)
                Gizmos.DrawLine(transform.position, transform.position + transform.right);
            //Gizmos.DrawLine(Vector3.zero, HeadForwardProjection);
            if (Head != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(Head.Transform.position, Head.Transform.position + Head.Transform.right);
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(Head.Transform.position, Head.Transform.position + Head.Transform.forward);
            }
        }
    }
}