using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draw : MonoBehaviour
{
    List<Vector3> linePoints;
    float timer;
    public float timerDelay;

    public GameObject brush;
    PhotonView view;
    LineRenderer drawLine;
    void Start()
    {
        linePoints = new List<Vector3>();
        timer = timerDelay;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "wall")
        {
            GameObject newLine = PhotonNetwork.Instantiate(brush.name, new Vector3(), Quaternion.identity);
            drawLine = newLine.GetComponent<LineRenderer>();
            view = newLine.GetComponent<PhotonView>();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "wall")
        {
            view.RPC("ModifyLine", RpcTarget.All);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "wall")
        {
            linePoints.Clear();
        }
    }

    [PunRPC]
    private void ModifyLine()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            linePoints.Add(transform.position);
            drawLine.positionCount = linePoints.Count;
            drawLine.SetPositions(linePoints.ToArray());

            timer = timerDelay;
        }
    }
}
