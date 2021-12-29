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

        [SynchronizeMe]
        public readonly Transform Transform;
        public Transform TargetingTransform { get; private set; } = null;
        private readonly ParentConstraint parentConstraint;
        private readonly IRigConstraint ikConstraint;

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
            parentConstraint.weight = target is null ? 0 : 1;
            //parentConstraint.constraintActive = target is null ? false : true;
            ikConstraint.weight = target is null ? 0 : 1;
            for (var i = parentConstraint.sourceCount - 1; i >= 0; i--)
                parentConstraint.RemoveSource(i);
            parentConstraint.AddSource(new ConstraintSource
            {
                sourceTransform = target,
                weight = 1
            });
        }

        [SynchronizeMe]
        public void StopFollowing() => StartFollowing(null);
    }
}