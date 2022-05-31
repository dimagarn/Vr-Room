using Photon.Pun;
using UnityEngine;

public class Brush : MonoBehaviour
{
    public GameObject brush;
    private PhotonView photonView;

    public void ChangeColor(Material newMaterial)
    {
        Debug.Log(newMaterial.ToString());
        brush.GetComponent< LineRenderer>().material = newMaterial;
    }
}
