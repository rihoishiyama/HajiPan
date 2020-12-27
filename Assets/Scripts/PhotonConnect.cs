using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class PhotonConnect : MonoBehaviourPunCallbacks
{
    //Connect関連
    private PhotonView m_photonView = null;
    private string m_roomName = "defaultRoom";
    private bool m_connectInUpdate = true;
    private bool m_IsMakeRoomFailed = false;

    //設定
    [SerializeField, TooltipAttribute("自動接続可否")]
    private bool m_autoConnect = true;
    private bool m_isManualOnConnect = false;
    [SerializeField]
    private const byte m_version = 1;

    //テキストコンポーネント
    [SerializeField] private Text m_userIdText;

    private Vector3[] m_startPos = { new Vector3(16, 0, 12), new Vector3(-16, 0, 12), new Vector3(-16, 0, -12), new Vector3(16, 0, -12) };

 //////////////////////////////////////////////////////////////////////////////////

    private void Start()
    {
        // PhotonServerSettingsに設定した内容を使ってマスターサーバーへ接続する
        PhotonNetwork.ConnectUsingSettings();
    }

    private void Update()
    {
        if (m_connectInUpdate && m_autoConnect && !PhotonNetwork.IsConnected)
        {
            Debug.Log("Update() was called by Unity. Scene is loaded. Let's connect to the Photon Master Server. Calling: PhotonNetwork.ConnectUsingSettings();");
            m_connectInUpdate = false;
            //PhotonNetwork.ConnectUsingSettings(m_version + "." + SceneManagerHelper.ActiveSceneBuildIndex);
            PhotonNetwork.ConnectUsingSettings();

        }
        if (m_isManualOnConnect)
        {
            OnConnectedToMaster();
            m_isManualOnConnect = false;
        }
    }

    // マスターサーバーへの接続が成功した時に呼ばれるコールバック
    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster() was called by PUN. Now this client is connected and could join a room. Calling: PhotonNetwork.JoinRandomRoom();");
        // "room"という名前のルームに参加する（ルームが無ければ作成してから参加する）
        //Debug.Log("WhenConnect...RoomName: " + matchingController.roomName + ", RandomJoin: " + matchingController.isRandomJoin + ", IsGetName: " + matchingController.isGetRoomName + ", isJoin: " + matchingController.isJoin + ", isCreate: " + matchingController.isCreate);

        //if (matchingController.isGetRoomName || matchingController.isRandomJoin)
        //{
        //    if (matchingController.isRandomJoin)
        //    {
        //        PhotonNetwork.JoinRandomRoom();
        //        Debug.Log("RandomRoomName" + ROOM_NAME);
        //    }
        //    else if (matchingController.isJoin)
        //    {
        //        // join失敗したらofflineになるかも?
        //        PhotonNetwork.JoinOrCreateRoom(ROOM_NAME, new RoomOptions() { MaxPlayers = 4 }, TypedLobby.Default);
        //        Debug.Log("JoinRoomCOmplete");
        //    }
        //    else
        //    {
        //        PhotonNetwork.CreateRoom(ROOM_NAME, new RoomOptions(), TypedLobby.Default);
        //        Debug.Log("CreateRoomComplete");
        //    }
        //}
        PhotonNetwork.JoinOrCreateRoom(m_roomName, new RoomOptions() { MaxPlayers = 4 }, TypedLobby.Default);
    }

    // マッチングが成功した時に呼ばれるコールバック
    public override void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom() called by PUN. Now this client is in a room. From here on, your game would be running. For reference, all callbacks are listed in enum: PhotonNetworkingMessage");
        SpawnObject();
    }

    public void OnPhotonCreateRoomFailed()
    {
        // ルーム作成失敗時
        Debug.Log("OnCreateRoom() called by PUN. But Failed so RoomName is exist.");
        m_isManualOnConnect = true;
        //matchingController.ReMatching();
        m_isManualOnConnect = true;
    }

    // Player生成
    public void SpawnObject()
    {
        //matchingController.LoadCanvas(true, false, true);

        //生成
        GameObject player = PhotonNetwork.Instantiate("TankPlayer", new Vector3(0, 0, 0), Quaternion.identity, 0);
        if (!player.GetComponent<Rigidbody>())
        {
            player.gameObject.AddComponent<Rigidbody>();
        }
        m_photonView = player.GetComponent<PhotonView>();
        //pos設定
        int ownerID = int.Parse(PhotonNetwork.LocalPlayer.UserId);
        Vector3 playerPos = m_startPos[(ownerID - 1) % 4];
        player.transform.position = playerPos;

        //ログ
        Debug.Log("Playerがスポーンされました。ower_id : " + ownerID);
        if(m_userIdText)
        {
            m_userIdText.text = ownerID.ToString();
        }
    }
}