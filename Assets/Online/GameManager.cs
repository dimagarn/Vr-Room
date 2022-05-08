using System;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] public Text textLastMessage;
    [SerializeField] public InputField textMessage;

    private PhotonView photonView;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    public void SendButton()
    {
        photonView.RPC("Send_Data", RpcTarget.AllBuffered,PhotonNetwork.NickName, textMessage.text);
    }

    [PunRPC]
    private void Send_Data(string  nick, string message)
    {
        textLastMessage.text += nick + ":" + message + "\r\n";
    }
}
