using UnityEngine;

public class Save : MonoBehaviour
{
    public SerializationController Controller;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Input.GetKeyDown(KeyCode.LeftShift))
            Controller.DeleteSerialization();
        if (Input.GetKeyDown(KeyCode.Space))
            Controller.Serialize();
    }
}
