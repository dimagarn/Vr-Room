using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Animations.Rigging;

namespace MeetingRoomVR.Character.Infrastructure
{
    public class TrackingIK
    {
        public const string HeadIKName = "HeadIK";
        public const string LeftHandIKName = "LeftHandIK";
        public const string RightHandIKName = "RightHandIK";
        public const string LeftFootIKName = "LeftFootIK";
        public const string RightFootIKName = "RightFootIK";

        [SynchronizeMe]
        public readonly Transform Transform;
        public Transform TargetingTransform { get; private set; } = null;
        private readonly ParentConstraint parentConstraint;
        private readonly IRigConstraint ikConstraint;
        private Vector3 rotationOffset;
        public Vector3 RotationOffset
        {
            get => rotationOffset;
            set
            {
                rotationOffset = value;
                UpdateRotationOffset();
            }
        }

        private void UpdateRotationOffset()
        {
            if (parentConstraint.rotationOffsets.Length > 0)
                parentConstraint.SetRotationOffset(0, RotationOffset);
        }

        public TrackingIK(Transform IKTransform)
        {
            this.Transform = IKTransform;
            if (!IKTransform.TryGetComponent<ParentConstraint>(out var parentConstraint))
                throw new System.ArgumentException($"Object {IKTransform.name} doesn't have ParentConstraint");
            this.parentConstraint = parentConstraint;
            var ikConstraint = IKTransform.GetComponentsInChildren<IRigConstraint>().Last();//must be some IK Constraint, controlling IKTransform
            this.ikConstraint = ikConstraint;
        }

        [SynchronizeMe]
        public void StartFollowing(Transform target)
        {
            TargetingTransform = target;
            //parentConstraint.constraintActive = target is null ? false : true;
            if (target == null)
            {
                parentConstraint.weight = 0;
                ikConstraint.weight = 0;
            }
            else
            {
                parentConstraint.weight = 1;
                ikConstraint.weight = 1;
            }
            for (var i = parentConstraint.sourceCount - 1; i >= 0; i--)
                parentConstraint.RemoveSource(i);
            parentConstraint.AddSource(new ConstraintSource
            {
                sourceTransform = target,
                weight = 1
            });
            UpdateRotationOffset();
        }

        [SynchronizeMe]
        public void StopFollowing() => StartFollowing(null);
    }
}