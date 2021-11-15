using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
/*
public class ServerDrawer : MonoBehaviour
{
    List<Vector3> linePoints;
    float timer;
    public float timerDelay;


    public GameObject Controller;
    public GameObject brush;
    //SerializationController controller;
    PhotonView view;
    LineRenderer drawLine;

    void Start()
    {
        //controller = Controller.GetComponent<SerializationController>();
        linePoints = new List<Vector3>();
        timer = timerDelay;
    }

    [PunRPC]
    public void StartLine()
    {
        GameObject newLine = PhotonNetwork.Instantiate(brush.name, new Vector3(), Quaternion.identity);
        drawLine = newLine.GetComponent<LineRenderer>();
    }

    [PunRPC]
    public void ModifyLine(Vector3 point)
    {
        linePoints.Add(point);
        drawLine.positionCount = linePoints.Count;
        drawLine.SetPositions(linePoints.ToArray());
    }

    [PunRPC]
    public void EndLine()
    {
        linePoints.Clear();
        drawLine = null;
    }

    [PunRPC]
    public bool IsDrawing()
    {
        return drawLine != null;
    }
}
*/