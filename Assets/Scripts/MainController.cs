using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

using Hashtable = ExitGames.Client.Photon.Hashtable;

public class MainController : MonoBehaviourPunCallbacks, IPunObservable
{
    // connect関連
    private PhotonView m_photonView = null;
    private bool m_connectInUpdate = true;

    // settings
    [SerializeField, TooltipAttribute("自動接続可否")]
    private bool m_autoConnect = true;
    private int m_playerID;

    // userIDカスタムプロパティ
    private const string PLAYER_ID = "UserId";
    private const int PLAYER_MAX_LIMIT = 4;

    //テキストコンポーネント
    private Text m_userIdText;
    private string m_updateUserIDText = "";

    //タンク生成初期値
    private Vector3[] m_startPos = { new Vector3(16, 0, 12), new Vector3(-16, 0, 12), new Vector3(-16, 0, -12), new Vector3(16, 0, -12) };
    private readonly Color[] m_material_colors = new Color[] { Color.red, Color.green, Color.yellow, Color.blue };


    //MainControllerで加えた
    [SerializeField]
    private Text roomNumText;
    [SerializeField]
    private Image blockTouchObj;
    [SerializeField]
    private Button startBtn;

    private bool isEnableStart = false;

    ////////////////////////////////////////////////////////////////////////////

    private void Start()
    {
        // シーン遷移用処理 ichikawa
        PhotonNetwork.IsMessageQueueRunning = true;
        Debug.Log(PhotonNetwork.CurrentRoom.Name + "に入室");
        
        // テスト
        Debug.Log("Start playerCnt: " + PhotonNetwork.CurrentRoom.PlayerCount);
        UpdateRoomCustomProperties(PhotonNetwork.CurrentRoom.PlayerCount);        

        // trueで操作不可, falseで操作可能
        blockTouchObj.raycastTarget = true;
        // falseでtouch不可
        startBtn.interactable = false;
        startBtn.onClick.AddListener(() => GameStartBtn());
        isEnableStart = false;

        /*
         * SpawnObject()のErrorでこの関数以降の処理が死ぬ.
         * →startの1行目におくとstart関数死ぬ. 
         * Debugで確認できる.
         */
        SpawnObject();
        /*
         * Error内容
        MissingComponentException: There is no 'Renderer' attached to the "TankPlayer(Clone)" game object, but a script is trying to access it.
        You probably need to add a Renderer to the game object "TankPlayer(Clone)". Or your script needs to check if the component is attached before using it.
        UnityEngine.Renderer.get_material () (at /Users/bokken/buildslave/unity/build/Runtime/Export/Graphics/GraphicsRenderers.bindings.cs:133)
        MainController.SpawnObject () (at Assets/Scripts/MainController.cs:186)
        MainController.Start () (at Assets/Scripts/MainController.cs:68)
         */
    }

    private void Update()
    {
        if (isEnableStart && PhotonNetwork.IsMasterClient)
        {
            Debug.Log("startBtn Activate");
            startBtn.interactable = true;
            // 毎回Updateで動いてしまうため
            isEnableStart = false;
        }
    }

    public void GameStartBtn()
    {
        // TODO
        // startのAnimationを追加したい
        Debug.Log("push StartBtn");
        blockTouchObj.raycastTarget = false;
        startBtn.gameObject.SetActive(false);
        Debug.Log("GameStart!!");
    }

    /*
     * roomの人数を更新したい  TODO
     * roomにCustomPropertiesを用いて人数を常に引用できるようにしたい.
     * hashtableの名前はplayerCnt
     * OnJoinedRoomはMenuControllerで記述したやつが動いて以降動かない. 
    */
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        var playerCnt = 0;
        Debug.Log("player入室");
        foreach (var p in PhotonNetwork.PlayerList)
        {
            playerCnt++;
        }
        UpdateRoomCustomProperties(playerCnt);

        // startBtn起動
        if (playerCnt == 4)
        {
            isEnableStart = true;
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        var playerCnt = 0;
        Debug.Log("player退出");
        foreach (var p in PhotonNetwork.PlayerList)
        {
            playerCnt++;
        }
        UpdateRoomCustomProperties(playerCnt);

        // startBtn不可
        if (playerCnt != 4)
        {
            isEnableStart = false;
            if (PhotonNetwork.IsMasterClient)
            {
                startBtn.interactable = false;
            }
        }
    }

    private void UpdateRoomCustomProperties(int playerCnt)
    {
        if (PhotonNetwork.InRoom)
        {
            Hashtable customProperties = PhotonNetwork.CurrentRoom.CustomProperties;
            customProperties["playerCnt"] = playerCnt;
            roomNumText.text = string.Format("{0}/{1}", playerCnt, 4);
            Debug.Log("Update Room playerCnt: " + playerCnt);

            PhotonNetwork.CurrentRoom.SetCustomProperties(customProperties);
        }
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
        //生成
        GameObject player = PhotonNetwork.Instantiate("TankPlayer", new Vector3(0, 0, 0), Quaternion.identity, 0);
        if (!player.GetComponent<Rigidbody>())
        {
            player.gameObject.AddComponent<Rigidbody>();
        }
        m_photonView = player.GetComponent<PhotonView>();

        SetPlayerID();

        //pos設定
        Vector3 playerPos = player.transform.position;
        playerPos = m_startPos[m_playerID];
        player.transform.position = playerPos;

        //カラー設定
        Renderer render = player.GetComponent<Renderer>();
        render.material.color = m_material_colors[m_playerID];

        Transform childObj = player.GetComponentInChildren<Transform>();
        foreach (Transform child in childObj)
        {
            foreach (Transform grandChild in child)
            {
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
    public void UpdatePlayerNum(Player player, int assignNum)
    {
        Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
        hashtable[PLAYER_ID] = assignNum;
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
                UpdatePlayerNum(PhotonNetwork.LocalPlayer, playerAssignNum);
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
            UpdatePlayerNum(PhotonNetwork.LocalPlayer, playerSetableCountList[0]);

            m_playerID = GetPlayerNum(PhotonNetwork.LocalPlayer);
        }
    }
}
