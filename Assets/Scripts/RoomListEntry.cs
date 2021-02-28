using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    private string roomName;

    void Start()
    {
        roomName = gameObject.name;
        Debug.Log(roomName);
        button.onClick.AddListener(() => PhotonNetwork.JoinOrCreateRoom(roomName, new RoomOptions() { MaxPlayers = 4 }, TypedLobby.Default));
    }

    public void Activate(RoomInfo info)
    {
        //Debug.Log("Activate");
        Debug.Log(info.Name + ": Activate!");

        //roomName = info.Name;

        // buttonの記述を変更
        string playerCounter = string.Format("{0}/{1}", info.PlayerCount, 4);
        buttonText.text = roomName + "\n" + playerCounter;

        // roomの人数が満員じゃない時可能
        button.interactable = (info.PlayerCount < 4);

        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
