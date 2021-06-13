using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class MenuController : MonoBehaviourPunCallbacks
{
    // RoomListを用いて部屋に入る
    // 
    //https://connect.unity.com/p/pun2deshi-meruonraingemukai-fa-ru-men-sono5
    [SerializeField]
    private GameObject startBtnObj;
    [SerializeField]
    private GameObject joinBtnObj;
    [SerializeField]
    private GameObject undoBtnObj;
    [SerializeField]
    private GameObject roomListObj;
    [SerializeField]
    private List<RoomListEntry> entryList;

    //テスト機能用
    //[SerializeField]
    //private GameObject testBtnObj;

    private enum MenuStatus
    {
        Start,
        Join,
        RoomList,
    };
    private MenuStatus status;

    private void Start()
    {
        StartInit();
        PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = "jp";
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("マスターサーバーへ接続しました");
        PhotonNetwork.JoinLobby();
    }

    //ロビーにいるときにRoomListは更新される
    public override void OnJoinedLobby()
    {
        Debug.Log("ロビーに参加しました");
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("ルームを作成しました");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("ルームに参加しました");

        PhotonNetwork.IsMessageQueueRunning = false;
        GameState.SetGameState(GameState.e_GameState.Mactting);
        SceneManager.LoadSceneAsync("MainGameScene", LoadSceneMode.Single);
    }

    public void StartInit(bool startFlag = true)
    {
        //デフォルト startbutton on
        startBtnObj.SetActive(startFlag);
        joinBtnObj.SetActive(false);
        roomListObj.SetActive(false);
        undoBtnObj.SetActive(false);
    }

    public void StartButton()
    {
        Debug.Log("push startButton");
        startBtnObj.SetActive(false);
        joinBtnObj.SetActive(true);
        undoBtnObj.SetActive(true);
        status = MenuStatus.Start;
    }

    public void JoinButton()
    {
        // roomlist join用
        joinBtnObj.SetActive(false);
        roomListObj.SetActive(true);
        undoBtnObj.SetActive(true);
        status = MenuStatus.Join;
    }

    public void UndoButton()
    {
        StartInit();
        switch (status)
        {
            case MenuStatus.Join:
                StartButton();
                break;
        }
    }

    //roomlistの情報が更新された時
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (var info in roomList)
        {
            foreach (var entry in entryList)
            {
                if (entry.gameObject.name.Equals(info.Name))
                    entry.Activate(info);
            }
        }
        Debug.Log("ListUpdate");
    }
}
