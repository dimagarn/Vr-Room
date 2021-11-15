using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPlayers : MonoBehaviour
{
    public GameObject player;
    public GameObject pen;
    public GameObject Controller;
    public GameObject Brush;
    private SerializationController controller;
    private static bool firstStart;

    void Start()
    {
        controller = Controller.GetComponent<SerializationController>();
        PhotonNetwork.Instantiate(player.name, new Vector3(), Quaternion.identity);
        if (firstStart)
        {
            PhotonNetwork.Instantiate(pen.name, new Vector3(-0.279f,1.085521f, 1.905365f), Quaternion.identity);
            controller.Deserialize(Brush);
            firstStart = false;
        }
    }
}
