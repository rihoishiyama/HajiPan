using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    private GameObject createBtnObj;
    [SerializeField]
    private GameObject selectJoinBtnObj;
    [SerializeField]
    private GameObject randomJoinBtnObj;
    [SerializeField]
    private GameObject undoBtnObj;
    [SerializeField]
    private GameObject testBtnObj;
    [SerializeField]
    private GameObject roomListObj;
    [SerializeField]
    private List<RoomListEntry> entryList;
    [SerializeField]
    private GameObject inputFieldObj;
    [SerializeField]
    private InputField inputRoomName;

    private enum MenuStatus
    {
        Start,
        Join,
        Create,
        SelectJoin,
        RandomJoin,

    };

    private MenuStatus status;
    private string roomName;

    private void Start()
    {
        StartInit();
        PhotonNetwork.ConnectUsingSettings();
        //OnConnectedToMaster();
    }

    // ロビーに参加
    public override void OnConnectedToMaster()
    {
        Debug.Log("マスターサーバーへ接続しました");
        PhotonNetwork.JoinLobby();
    }

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
        
    }

    public void StartInit(bool startFlag = true)
    {
        startBtnObj.SetActive(startFlag);
        joinBtnObj.SetActive(false);
        createBtnObj.SetActive(false);
        selectJoinBtnObj.SetActive(false);
        randomJoinBtnObj.SetActive(false);
        undoBtnObj.SetActive(false);
        inputFieldObj.SetActive(false);
    }

    public void StartButton()
    {
        Debug.Log("push startButton");
        startBtnObj.SetActive(false);
        joinBtnObj.SetActive(true);
        //createBtnObj.SetActive(true);
        undoBtnObj.SetActive(true);
        status = MenuStatus.Start;
    }

    public void JoinButton()
    {
        // roomlistよう
        joinBtnObj.SetActive(false);
        roomListObj.SetActive(true);

        // join,createで分ける場合
        //joinBtnObj.SetActive(false);
        //createBtnObj.SetActive(false);

        //selectJoinBtnObj.SetActive(true);
        //randomJoinBtnObj.SetActive(true);
        //undoBtnObj.SetActive(true);
        //status = MenuStatus.Join;
    }

    public void SelectJoinButton()
    {
        selectJoinBtnObj.SetActive(false);
        randomJoinBtnObj.SetActive(false);

        inputFieldObj.SetActive(true);
        undoBtnObj.SetActive(true);
        status = MenuStatus.SelectJoin;
    }

    public void RandomJoinButton()
    {
        selectJoinBtnObj.SetActive(false);
        randomJoinBtnObj.SetActive(false);
        undoBtnObj.SetActive(false);
        status = MenuStatus.RandomJoin;
    }

    public void CreateButton()
    {
        joinBtnObj.SetActive(false);
        createBtnObj.SetActive(false);

        inputFieldObj.SetActive(true);
        undoBtnObj.SetActive(true);
        status = MenuStatus.Create;
    }

    public void EnterButton()
    {
        roomName = inputRoomName.text;
        Debug.Log(roomName);
    }

    public void UndoButton()
    {
        StartInit();
        switch (status)
        {
            case MenuStatus.Join:
                StartButton();
                break;
            case MenuStatus.Create:
                StartButton();
                break;
            case MenuStatus.SelectJoin:
                StartInit(false);
                JoinButton();
                break;
            case MenuStatus.RandomJoin:
                StartInit(false);
                JoinButton();
                break;
        }
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (var info in roomList)
        {
            //Debug.Log(info.Name);
            foreach (var entry in entryList)
            {
                if (entry.gameObject.name.Equals(info.Name))
                    entry.Activate(info);
            }
        }
        Debug.Log("ListUpdate");
    }
}
