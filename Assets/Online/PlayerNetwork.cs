using System;
using UnityEngine;
using UnityEngine.XR;
using Photon.Pun;
using Valve.VR;
using Valve.VR.InteractionSystem;
using Hand = Valve.VR.InteractionSystem.Hand;

public class PlayerNetwork : MonoBehaviour
{
    public GameObject noneVRContent;
    public GameObject input;
    public GameObject noneVRHand;
    public GameObject VRLeftHand;
    public GameObject VRRightHand;
    public GameObject VRCamera;
    public GameObject VR;
    private PhotonView photonView;
    private static PlayerNetwork _playerNetwork;

    // Start is called before the first frame update
    private void Awake()
    {
        
        photonView = GetComponent<PhotonView>();
        if (!photonView.IsMine)
        {
            input.SetActive(false);
            noneVRHand.GetComponent<Hand>().enabled = false;
            noneVRContent.GetComponent<Camera>().enabled = false;
            noneVRContent.GetComponent<FallbackCameraController>().enabled = false;
            //GetComponent<Player>().enabled = false;

            //VR.SetActive(false);
            VRCamera.GetComponent<Camera>().enabled = false;
            VRCamera.GetComponent<SteamVR_Fade>().enabled = false;
            VRLeftHand.GetComponent<Hand>().enabled = false;
            VRRightHand.GetComponent<Hand>().enabled = false;
        }
    }
}
