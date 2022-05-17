using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Draw : MonoBehaviour
{
    List<Vector3> linePoints;
    float timer;
    public float timerDelay;
    private Rigidbody pencilRig;

    public GameObject brush;
    [PunRPC] private LineRenderer drawLine;
    [PunRPC] private GameObject newLine;
    
    private PhotonView photonView;
    void Start()
    {
        linePoints = new List<Vector3>();
        timer = timerDelay;
        pencilRig = GetComponent<Rigidbody>();
        photonView = GetComponent<PhotonView>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(photonView.IsMine);
        if (other.tag == "wall" && photonView.IsMine)
        {
            photonView.RPC("StartDraw", RpcTarget.AllBuffered);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "wall" && photonView.IsMine)
        {
            photonView.RPC("OnDrawing", RpcTarget.AllBuffered);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "wall" && photonView.IsMine)
        {
            photonView.RPC("EndDraw", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    private void StartDraw()
    {
        newLine = Instantiate(brush);
        drawLine = newLine.GetComponent<LineRenderer>();
        pencilRig.freezeRotation = true;
    }

    [PunRPC]
    private void OnDrawing()
    {
        linePoints.Add(transform.position + new Vector3(0.02f, 0f, 0.07f));
        drawLine.positionCount = linePoints.Count;
        drawLine.SetPositions(linePoints.ToArray());

        timer = timerDelay;
    }

    [PunRPC]
    private void EndDraw()
    {
        var meshCollider = newLine.AddComponent<MeshCollider>();
        var mesh = new Mesh();
        drawLine.BakeMesh(mesh, true);
        meshCollider.sharedMesh = mesh;
        meshCollider.convex = true;
        meshCollider.isTrigger = true;
        linePoints.Clear();
        pencilRig.freezeRotation = false;
    }
}
