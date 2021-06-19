using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class RoomListEntry : MonoBehaviour
{
    [SerializeField]
    private Button button;
    [SerializeField]
    private Text buttonText;
    [SerializeField]
    private string roomName;

    void Start()
    {
        Debug.Log(roomName);
        button.onClick.AddListener(() => PhotonNetwork.JoinOrCreateRoom(roomName, new RoomOptions() { MaxPlayers = 4 , IsOpen = true}, TypedLobby.Default));
    }

    public void Activate(RoomInfo info)
    {
        //Debug.Log("Activate");
        Debug.Log(roomName + ": Activate!");

        // buttonの記述を変更
        string playerCounter = string.Format("{0}/{1}", info.PlayerCount, 4);
        buttonText.text = roomName + "\n" + playerCounter;

        // roomの人数が満員じゃないかつisOpenの時
        button.interactable = (info.PlayerCount < 4 && info.IsOpen);

        gameObject.SetActive(true);
    }
}
