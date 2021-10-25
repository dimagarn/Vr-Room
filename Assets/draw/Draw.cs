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
    //SerializationController controller;
    PhotonView view;
    LineRenderer drawLine;

    void Start()
    {
        //controller = Controller.GetComponent<SerializationController>();
        linePoints = new List<Vector3>();
        timer = timerDelay;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "wall")
        {
            GameObject newLine = PhotonNetwork.Instantiate(brush.name, new Vector3(), Quaternion.identity);
            drawLine = newLine.GetComponent<LineRenderer>();
            view = GetComponentInParent<PhotonView>();
            // Тоже выделить PunRPC чтобы избавиться от проблем с прерываниями линии?
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "wall")
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                //linePoints.Add(transform.position);
                //drawLine.positionCount = linePoints.Count;
                //drawLine.SetPositions(linePoints.ToArray());
                view.RPC("ModifyLine", RpcTarget.All, transform.position);

                timer = timerDelay;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "wall")
        {
            linePoints.Clear();
            //controller.AddLine(drawLine);
        }
    }

    [PunRPC]
    public void ModifyLine(Vector3 point)
    {
        linePoints.Add(point);
        drawLine.positionCount = linePoints.Count;
        drawLine.SetPositions(linePoints.ToArray());
    }
}
