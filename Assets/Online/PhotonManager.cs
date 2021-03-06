using System;
using System.Collections;
using System.Collections.Generic;
using MeetingRoomVR.Character;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using Valve.VR;
using Player = Valve.VR.InteractionSystem.Player;

public class PhotonManager : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField] public string region;
    [SerializeField] public string nickName;
    [SerializeField] public InputField roomName;
    [SerializeField] public ListItem itemPrefab;
    [SerializeField] public Transform content;
    
    public List<RoomInfo> allRoomInfo = new List<RoomInfo>();

    [SerializeField] public GameObject player_prefab;
    [SerializeField] public GameObject playerC;
    [SerializeField] public GameObject sController;
    [SerializeField] public GameObject brush;

    public GameObject follow;
    private SerializationController serial;
    private GameObject serverPlayer;
    private Player plyer;
    private AnimatedAvatar avatar;
    private GameObject leftHand;
    private GameObject rightHand;

    private void Awake()
    {
        if (SceneManager.GetActiveScene().name == "SampleScene")
        {
            serverPlayer = PhotonNetwork.Instantiate(player_prefab.name, Vector3.zero, Quaternion.identity);
            if (SteamVR.instance != null)
            {
                avatar = serverPlayer.transform.Find("Avatar").GetComponent<AnimatedAvatar>();
                avatar.HideHands = true;
                plyer = playerC.GetComponent<Player>();
                StartCoroutine(GetHandsPosition());
                leftHand = serverPlayer.transform.Find("leftHand").gameObject;
                rightHand = serverPlayer.transform.Find("rightHand").gameObject;
            }
            serial = sController.GetComponent<SerializationController>();
            serial.CreateSerializer(PhotonNetwork.CurrentRoom.Name);
            serial.Deserialize(brush);
        }
    }

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.ConnectToRegion(region);
    }

    private void Update()
    {
        if (serverPlayer != null)
        {
            serverPlayer.transform.Find("head").transform.position = follow.transform.position;
            serverPlayer.transform.Find("head").transform.rotation = follow.transform.rotation;
            
            if (plyer && plyer.leftHand && plyer.rightHand)
            {
                leftHand.transform.position = plyer.leftHand.skeleton.wrist.position;
                leftHand.transform.rotation = plyer.leftHand.skeleton.wrist.rotation;
                rightHand.transform.position = plyer.rightHand.skeleton.wrist.position;
                rightHand.transform.rotation = plyer.rightHand.skeleton.wrist.rotation;
            }
        }
    }
    
    private IEnumerator GetHandsPosition()
    {
        while (true)
        {
            if (leftHand == null ||
                rightHand == null)
                yield return new WaitForSeconds(1f);
            else
            {
                avatar.LeftHand.StartFollowing(leftHand.transform);
                avatar.RightHand.StartFollowing(rightHand.transform);
                yield break;
            }
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("???? ???????????????????? ?? ?????????????? " + PhotonNetwork.CloudRegion);
        if (nickName == "")
        {
            PhotonNetwork.NickName = "User";
        }
        else PhotonNetwork.NickName = nickName;
        
        if(!PhotonNetwork.InLobby) 
            PhotonNetwork.JoinLobby();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("???? ?????????????????? ???? ??????????????");
    }

    public void CreateRoomButton()
    {
        if(!PhotonNetwork.IsConnected)
            return;
        var roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 3;
        PhotonNetwork.JoinOrCreateRoom(roomName.text, roomOptions, TypedLobby.Default);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("?????????????? ?????????????? " + PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError("???? ?????????????? ?????????????? ??????????????");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (var info in roomList)
        {
            for (var i = 0; i < allRoomInfo.Count; i++)
            {
                if(allRoomInfo[i].masterClientId == info.masterClientId)
                    return;
            }

            var listItem = Instantiate(itemPrefab, content);
            if (listItem != null)
            {
                listItem.SetInfo(info);
                allRoomInfo.Add(info);
            }
        }
    }

    public override void OnJoinedRoom()
    {
        if(SteamVR.instance != null)
            Destroy(playerC);
        PhotonNetwork.LoadLevel("SampleScene");
    }

    public void LeaveButton()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel("Menu");
        PhotonNetwork.Destroy(playerC.gameObject);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            //stream.SendNext(serial);
        }
        else
        {
            //serial = (SerializationController)stream.ReceiveNext();
        }
    }
}