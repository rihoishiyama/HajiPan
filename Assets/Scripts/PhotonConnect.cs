using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PhotonConnect : MonoBehaviourPunCallbacks, IPunObservable
{
    //[SerializeField]
    //private ButtonController matchingController = null;

    //Connect関連
    private PhotonView m_photonView = null;
    private string m_roomName = "defaultRoom";
    private bool m_connectInUpdate = true;
    private bool m_IsMakeRoomFailed = false;

    //設定
    [SerializeField, TooltipAttribute("自動接続可否")]
    private bool m_autoConnect = true;
    private bool m_isManualOnConnect = false;
    [SerializeField] private const byte m_version = 1;
    [SerializeField] private int m_playerID;

    //userIDカスタムプロパティ
    private const string PLAYER_ID = "UserId";
    private const string IS_DEAD = "IsDead";
    private const int PLAYER_MAX_LIMIT = 4;

    //テキストコンポーネント
    private Text m_userIdText;
    private string m_updateUserIDText = "";

    //タンク生成初期値
    private Vector3[] m_startPos = { new Vector3(16, 0, 12), new Vector3(-16, 0, 12), new Vector3(-16, 0, -12), new Vector3(16, 0, -12) };
    private readonly Color[] m_material_colors = new Color[] { Color.red, Color.green,　Color.yellow, Color.blue };

    //ゲームオーバー管理
    [SerializeField] private GameOver m_gameOver;

    //////////////////////////////////////////////////////////////////////////////////

    private void Start()
    {
        //シーン遷移用処理 市川
        PhotonNetwork.IsMessageQueueRunning = true;
        SpawnObject();
        // PhotonServerSettingsに設定した内容を使ってマスターサーバーへ接続する
        //PhotonNetwork.ConnectUsingSettings();
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

        if (m_photonView != null)
        {

            if (m_photonView.IsMine) //ここもTank側
            {
                //確認作業のためコメントアウト 市川
                //Debug.Log(m_updateUserIDText);
            }

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

        //シーン遷移用 市川
        //PhotonNetwork.JoinOrCreateRoom(m_roomName, new RoomOptions() { MaxPlayers = PLAYER_MAX_LIMIT }, TypedLobby.Default);
    }

    // マッチングが成功した時に呼ばれるコールバック
    public override void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom() called by PUN. Now this client is in a room. From here on, your game would be running. For reference, all callbacks are listed in enum: PhotonNetworkingMessage");
        //SpawnObject();
    }

    public void OnPhotonCreateRoomFailed()
    {
        // ルーム作成失敗時
        Debug.Log("OnCreateRoom() called by PUN. But Failed so RoomName is exist.");
        m_isManualOnConnect = true;
        //matchingController.ReMatching();
        m_isManualOnConnect = true;
    }

    //情報共有 Tank側に書くべき？
    void IPunObservable.OnPhotonSerializeView(PhotonStream i_stream, PhotonMessageInfo i_info)
    {
        if (i_stream.IsWriting)
        {
            //データの送信
            i_stream.SendNext(m_userIdText);
            i_stream.SendNext(m_material_colors[m_playerID]);
        }
        else
        {
            //データの受信
            Text userIDText = (Text)i_stream.ReceiveNext();
            Color materialColor = (Color)i_stream.ReceiveNext();

            m_updateUserIDText = userIDText.text;
        }
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

        SetPlayerID();
        player.name = m_playerID + "_TankPlayer"; 
        m_gameOver.CreateMyTank(player, m_playerID);

        //pos設定
        Vector3 playerPos = player.transform.position;
        playerPos = m_startPos[m_playerID];
        player.transform.position = playerPos;

        //カラー設定
        Renderer render = player.GetComponent<Renderer>();
        render.material.color = m_material_colors[m_playerID];

        //Tankの上にUserIDを表示させる
        Transform childObj = player.GetComponentInChildren<Transform>();
        foreach (Transform child in childObj)
        {
            foreach (Transform grandChild in child)
            {
                // tankObjの構造が変更されたので修正必要
                if (grandChild.name == "UserID")
                {
                    m_userIdText = grandChild.GetComponent<Text>();
                    //ログ
                    Debug.Log("Playerがスポーンされました。player_id : " + m_playerID);
                    if (m_userIdText)
                    {
                        m_userIdText.text = "ID : " + m_playerID.ToString();
                    }
                }
            }
        }
    }

    // プレイヤーのカスタムプロパティが更新された時に呼ばれるコールバック
    public override void OnPlayerPropertiesUpdate(Player target, Hashtable changedProps)
    {
        // 更新されたキーと値のペアを、デバッグログに出力する
        foreach (var p in changedProps)
        {
            Debug.Log($"{p.Key}: {p.Value}");
        }
    }

    //プレイヤー番号を取得する
    public int GetPlayerNum(Player player)
    {
        int userId = (PhotonNetwork.LocalPlayer.CustomProperties[PLAYER_ID] is int value) ? value : 0;
        return userId;
    }

    //プレイヤーの割り当て番号のカスタムプロパティを更新する
    public void UpdateCustomProperty(Player player, int assignNum, bool isDead)
    {
        Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
        hashtable[PLAYER_ID] = assignNum;
        hashtable[IS_DEAD] = isDead;
        player.SetCustomProperties(hashtable);
    }

    // PlayerID付与
    private void SetPlayerID()
    {
        if (m_photonView.IsMine)
        {
            List<int> playerSetableCountList = new List<int>();

            //制限人数までの数字のリストを作成
            int count = 0;
            for (int i = 0; i < PLAYER_MAX_LIMIT; i++)
            {
                playerSetableCountList.Add(count);
                count++;
            }

            Player[] otherPlayers = PhotonNetwork.PlayerListOthers;

            //他のプレイヤーがいなければカスタムプロパティの値を"0"に設定
            if (otherPlayers.Length <= 0)
            {
                int playerAssignNum = otherPlayers.Length;
                UpdateCustomProperty(PhotonNetwork.LocalPlayer, playerAssignNum, false);
                m_playerID = GetPlayerNum(PhotonNetwork.LocalPlayer);

                return;
            }

            //他のプレイヤーのカスタムプロパティー取得してリスト作成
            List<int> playerAssignNums = new List<int>();
            for (int i = 0; i < otherPlayers.Length; i++)
            {
                playerAssignNums.Add(GetPlayerNum(otherPlayers[i]));
            }

            //リスト同士を比較し、未使用の数字のリストを作成
            playerSetableCountList.RemoveAll(playerAssignNums.Contains);

            //ローカルのプレイヤーのカスタムプロパティを設定
            //空いている場所のうち、一番若い数字の箇所を利用
            UpdateCustomProperty(PhotonNetwork.LocalPlayer, playerSetableCountList[0], false);

            m_playerID = GetPlayerNum(PhotonNetwork.LocalPlayer);
        }
    }
}
