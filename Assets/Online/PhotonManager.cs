using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    [SerializeField] public string region;
    [SerializeField] public InputField roomName;
    [SerializeField] public ListItem itemPrefab;
    [SerializeField] public Transform content;
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.ConnectToRegion(region);
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Вы подключены к серверу " + PhotonNetwork.CloudRegion);
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
        roomOptions.MaxPlayers = 2;
        PhotonNetwork.JoinOrCreateRoom(roomName.text, roomOptions, TypedLobby.Default);
        PhotonNetwork.LoadLevel("SampleScene");
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
            var listItem = Instantiate(itemPrefab, content);
            if(listItem != null)
                listItem.SetInfo(info);
        }
    }
}
