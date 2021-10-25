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
    LineRenderer drawLine;

    void Start()
    {
        controller = Controller.GetComponent<SerializationController>();
        linePoints = new List<Vector3>();
        timer = timerDelay;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "wall")
        {
            GameObject newLine = Instantiate(brush);
            drawLine = newLine.GetComponent<LineRenderer>();
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
            controller.AddLine(drawLine);
        }
    }
}
