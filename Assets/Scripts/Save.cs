using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Save : MonoBehaviour
{
    public SerializationController Controller;
    // Start is called before the first frame update
    void Start()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Space))
                Controller.Serialize();
        }
    }
}
