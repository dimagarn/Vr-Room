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
        public float StandingHeadHeight { get; private set; } = 1.7f;
        public float CrouchingHeadHeight { get; private set; } = 1.2f;
        public TrackingIK Head { get; private set; }
        public TrackingIK LeftHand { get; private set; }
        public TrackingIK RightHand { get; private set; }
        public Transform transform { get; private set; }
        private Animator animator;
        private Animator cloneAnimator;
        private MultiPositionConstraint hipsPositionConstraint;
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
        }

        private void SetAnimatorFloat(int hashedId, float value)
        {
            animator.SetFloat(hashedId, value);
            cloneAnimator.SetFloat(hashedId, value);
        }

        private void SetCrouchState(float crouchValue)
        {
            SetAnimatorFloat(crouchFloatHash, crouchValue);
            var hipsZOffset = Mathf.LerpUnclamped(0, 0.23f, Vector3.Dot(transform.up, Head.Transform.forward) * (crouchValue + 1));
            hipsPositionConstraint.data.offset = new Vector3(0, -0.59f, hipsZOffset);
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