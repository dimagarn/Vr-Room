using UnityEngine;
using UnityEngine.XR;
using Photon.Pun;

public class PlayerNetwork : MonoBehaviour
{
    public GameObject dude;
    public GameObject input;
    public GameObject hand;
    private PhotonView photonView;
    
    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();
        if (!photonView.IsMine)
        {
            input.SetActive(false);
            dude.SetActive(false);
            hand.SetActive(false);
        }
    }
}
