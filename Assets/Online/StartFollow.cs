using System.Collections;
using System.Collections.Generic;
using MeetingRoomVR.Character;
using UnityEngine;
using Photon.Pun;

public class StartFollow : MonoBehaviour
{
    public AnimatedAvatar avatar;
    public GameObject leftHand;
    public GameObject rightHand;
    private PhotonView photonView;
    
    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();
        if (!photonView.IsMine)
        {
            avatar.LeftHand.StartFollowing(leftHand.transform);
            avatar.RightHand.StartFollowing(rightHand.transform);
        }
    }
}
