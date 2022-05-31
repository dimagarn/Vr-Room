using Photon.Pun;
using UnityEngine;

public class Brush : MonoBehaviour
{
    public GameObject brush;
    private PhotonView photonView;

    public void ChangeColor(Material newMaterial)
    {
        photonView.RPC("CreateLine", RpcTarget.AllBuffered, newMaterial);
    }
    
    [PunRPC]
    private void CreateLine(Material newMaterial)
    {
        Debug.Log(newMaterial.ToString());
        brush.GetComponent< LineRenderer>().material = newMaterial;
    }
}
