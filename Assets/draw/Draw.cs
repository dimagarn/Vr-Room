using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draw : MonoBehaviour
{
    List<Vector3> linePoints;
    float timer;
    public float timerDelay;

    public GameObject Controller;
    public GameObject brush;
    SerializationController controller;
    PhotonView view;
    LineRenderer drawLine;
    bool isDrawing = false;

    void Start()
    {
        controller = Controller.GetComponent<SerializationController>();
        linePoints = new List<Vector3>();
        timer = timerDelay;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isDrawing)
            return;
        if (other.tag == "wall")
        {
            view = GetComponentInParent<PhotonView>();
            view.RPC("StartLine", RpcTarget.AllBuffered);
            isDrawing = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (isDrawing)
            return;
        if (other.tag == "wall")
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                //linePoints.Add(transform.position);
                //drawLine.positionCount = linePoints.Count;
                //drawLine.SetPositions(linePoints.ToArray());
                view.RPC("ModifyLine", RpcTarget.AllBuffered, transform.position);

                timer = timerDelay;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (isDrawing)
            return;
        if (other.tag == "wall")
        {
            view.RPC("EndLine", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    public void StartLine()
    {
        isDrawing = true;
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
        controller.AddLine(linePoints.ToArray());
        linePoints.Clear();
        drawLine = null;
        isDrawing = false;
    }
}
