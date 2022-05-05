using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;

public class ListItem : MonoBehaviour
{
    [SerializeField] public Text textName;
    [SerializeField] public Text textPlayerCount;

    public void SetInfo(RoomInfo info)
    {
        textName.text = info.Name;
        textPlayerCount.text = info.PlayerCount + "/" + info.MaxPlayers;
    }
}
