using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnUI : MonoBehaviour
{
    public GameObject UI;
    private GameObject UIClone;

    public void Spawn()
    {
        UIClone = Instantiate(UI, GetComponent<Transform>().position, GetComponent<Transform>().rotation);
    }

    public void Destroy()
    {
        GameObject.Destroy(UIClone);
    }
}
