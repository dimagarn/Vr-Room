using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using Valve.VR;

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

    private void Awake()
    {
        if (SceneManager.GetActiveScene().name == "SampleScene")
        {
            //player_prefab.transform.position = player.transform.position;
            serverPlayer = PhotonNetwork.Instantiate(player_prefab.name, Vector3.zero, Quaternion.identity);
            serial = sController.GetComponent<SerializationController>();
            serial.CreateSerializer(PhotonNetwork.CurrentRoom.Name);
            serial.Deserialize(brush);
        }
    }

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.ConnectToRegion(region);

        if (SceneManager.GetActiveScene().name == "SampleScene")
        {
           //PhotonNetwork.Instantiate(player_prefab.name, new Vector3(2.512f, -0.292f, 6.963f), Quaternion.identity);
        }
    }

    private void Update()
    {
        serverPlayer.transform.position = follow.transform.position;
        serverPlayer.transform.rotation = follow.transform.rotation;
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Вы подключены к серверу " + PhotonNetwork.CloudRegion);
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
        Debug.Log("Вы отключены от сервера");
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
        Debug.Log("Создана комната " + PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Не удалось создать комнату");
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