using System.Collections;
using System.Collections.Generic;
using MeetingRoomVR.Character;
using Photon.Pun;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class VRbinder : MonoBehaviour
{
    public Player plyer;
    public AnimatedAvatar avatar;
    private PhotonView photonView;
    
    // Start is called before the first frame update
    void Start()
    {
        if(GetComponent<PhotonView>() != null)
            photonView = GetComponent<PhotonView>();
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
            if (photonView.IsMine || photonView == null)
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
}
