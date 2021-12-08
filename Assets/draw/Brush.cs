using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brush : MonoBehaviour
{
    public GameObject brush;

    public void ChangeColor(Material newMaterial)
    {
        Debug.Log(newMaterial.ToString());
        brush.GetComponent< LineRenderer>().material = newMaterial;
    }
}
