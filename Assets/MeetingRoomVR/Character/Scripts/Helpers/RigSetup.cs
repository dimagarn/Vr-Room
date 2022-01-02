using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using MeetingRoomVR.Character;
using System.Linq;
using MeetingRoomVR.Character.Infrastructure;
using UnityEngine.Animations;

public class RigSetup : MonoBehaviour
{
    public Animator TargetAnimator;
    public Animator CloneAnimator;
    public bool TrySetup()
    {
        if (TargetAnimator == null || !TargetAnimator.TryGetComponent<RigBuilder>(out _) || CloneAnimator == null)
        {
            Debug.Log("Couldn't set up the rig.");
            return false;
        }
        try
        {
            Setup();
            Debug.Log("Rig set.");
            return true;
        }
        catch
        {
            Debug.Log("Couldn't set up the rig.");
            return false;
        }
    }

    private void Setup()
    {
        var animator = TargetAnimator;
        var rigBuilder = TargetAnimator.GetComponent<RigBuilder>();
        var rigObject = rigBuilder.layers[0].rig;
        foreach (var rigChild in rigObject.GetComponentsInChildren<Transform>())
        {
            switch (rigChild.name)
            {
                case TrackingIK.HeadIKName:
                    rigChild.GetComponentInChildren<MultiRotationConstraint>().data.constrainedObject = animator.GetBoneTransform(HumanBodyBones.Chest);
                    rigChild.GetComponentInChildren<MultiPositionConstraint>().data.constrainedObject = animator.GetBoneTransform(HumanBodyBones.Hips);
                    var chainHead = rigChild.GetComponentInChildren<ChainIKConstraint>();
                    chainHead.data.tip = animator.GetBoneTransform(HumanBodyBones.Head);
                    chainHead.data.root = animator.GetBoneTransform(HumanBodyBones.Hips);
                    break;
                case TrackingIK.LeftHandIKName:
                    var leftHand = rigChild.GetComponent<TwoBoneIKConstraint>();
                    leftHand.data.tip = animator.GetBoneTransform(HumanBodyBones.LeftHand);
                    leftHand.data.mid = animator.GetBoneTransform(HumanBodyBones.LeftLowerArm);
                    leftHand.data.root = animator.GetBoneTransform(HumanBodyBones.LeftUpperArm);
                    break;
                case TrackingIK.RightHandIKName:
                    var rightHand = rigChild.GetComponent<TwoBoneIKConstraint>();
                    rightHand.data.tip = animator.GetBoneTransform(HumanBodyBones.RightHand);
                    rightHand.data.mid = animator.GetBoneTransform(HumanBodyBones.RightLowerArm);
                    rightHand.data.root = animator.GetBoneTransform(HumanBodyBones.RightUpperArm);
                    break;
                case TrackingIK.LeftFootIKName:
                    var leftFoot = rigChild.GetComponent<TwoBoneIKConstraint>();
                    leftFoot.data.target = CloneAnimator.GetBoneTransform(HumanBodyBones.LeftFoot);
                    leftFoot.data.tip = animator.GetBoneTransform(HumanBodyBones.LeftFoot);
                    leftFoot.data.mid = animator.GetBoneTransform(HumanBodyBones.LeftLowerLeg);
                    leftFoot.data.root = animator.GetBoneTransform(HumanBodyBones.LeftUpperLeg);
                    break;
                case TrackingIK.RightFootIKName:
                    var rightFoot = rigChild.GetComponent<TwoBoneIKConstraint>();
                    rightFoot.data.target = CloneAnimator.GetBoneTransform(HumanBodyBones.RightFoot);
                    rightFoot.data.tip = animator.GetBoneTransform(HumanBodyBones.RightFoot);
                    rightFoot.data.mid = animator.GetBoneTransform(HumanBodyBones.RightLowerLeg);
                    rightFoot.data.root = animator.GetBoneTransform(HumanBodyBones.RightUpperLeg);
                    break;
            }
        }
    }
}
