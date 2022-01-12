using System.Collections;
using System.Collections.Generic;
using MeetingRoomVR.Character;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class VRbinder : MonoBehaviour
{
    public Player plyer;
    public AnimatedAvatar avatar;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(cor());
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private IEnumerator cor()
    {
        while (true)
        {
            var leftHand = plyer.leftHand;
            var rightHand = plyer.rightHand;
            if (leftHand == null ||
                rightHand == null ||
                leftHand.skeleton == null ||
                rightHand.skeleton == null)
                yield return new WaitForSeconds(1f);
            else
            {
                avatar.LeftHand.StartFollowing(leftHand.skeleton.wrist);
                avatar.RightHand.StartFollowing(rightHand.skeleton.wrist);
                yield break;
            }
        }
    }
}
