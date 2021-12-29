using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eraser : MonoBehaviour
{
    List<Vector3> linePoints;
    float timer;
    public float timerDelay;
    private Rigidbody pencilRig;

    public GameObject brush;
    private LineRenderer drawLine;
    private GameObject newLine;
    private List<GameObject> listToDestroy;
    void Start()
    {
        linePoints = new List<Vector3>();
        timer = timerDelay;
        pencilRig = GetComponentInParent<Rigidbody>();
        listToDestroy = new List<GameObject>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "wall")
        {
            newLine = Instantiate(brush);
            drawLine = newLine.GetComponent<LineRenderer>();
            pencilRig.freezeRotation = true;
        }

        else if (other.tag == "brush")
        {
            listToDestroy.Add(other.gameObject);
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
            linePoints.Clear();
            pencilRig.freezeRotation = false;
            Destroy(newLine, .1f);
            foreach (var e in listToDestroy)
                Destroy(e, .1f);
        }
    }
}
