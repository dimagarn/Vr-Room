using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draw : MonoBehaviour
{
    List<Vector3> linePoints;
    float timer;
    public float timerDelay;
    private Rigidbody pencilRig;

    public GameObject brush;
    private LineRenderer drawLine;
    private GameObject newLine;
    void Start()
    {
        linePoints = new List<Vector3>();
        timer = timerDelay;
        pencilRig = GetComponentInParent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "wall")
        {
            newLine = Instantiate(brush);
            drawLine = newLine.GetComponent<LineRenderer>();
            pencilRig.freezeRotation = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "wall")
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

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "wall")
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
}
