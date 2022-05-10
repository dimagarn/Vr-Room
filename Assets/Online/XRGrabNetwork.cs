using UnityEngine;
using Photon.Pun;

public class XRGrabNetwork : MonoBehaviour
{
    private PhotonView photonView;
    private Rigidbody rigidbody;
    [PunRPC] public bool onAttached;

    void Start()
    {
        photonView = GetComponent<PhotonView>();
        rigidbody = GetComponent<Rigidbody>();
    }

    public void Update()
    {
        if (onAttached)
            rigidbody.useGravity = false;
        else rigidbody.useGravity = true;
    }

    protected virtual void OnAttachedToHand()
    {
        photonView.RequestOwnership();
        photonView.RPC("Send_Data", RpcTarget.AllBuffered, true);
        onAttached = true;
    }

    protected virtual void OnDetachedFromHand()
    {
        photonView.RPC("Send_Data", RpcTarget.AllBuffered, false);
        onAttached = false;
    }
    
    [PunRPC]
    private void Send_Data(bool isAttached)
    {
        onAttached = isAttached;
    }
}
