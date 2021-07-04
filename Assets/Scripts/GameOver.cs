using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class GameOver : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject m_tankPlayer;
    [SerializeField] private GameObject m_gameOverPanel;
    [SerializeField] private GameObject m_judgeTextObj;
    [SerializeField] private GameObject m_exitBtn;
    [SerializeField] private GameObject m_watchBtn;
    [SerializeField] private GameObject m_countdownTextObj;

    private Text m_judgeText;
    private Text m_countdownText;

    private bool m_isCreateTank = false;
    private float m_countdown = 10.0f;

    void Awake()
    {
        m_gameOverPanel.SetActive(false);
        m_judgeTextObj.SetActive(false);
        m_exitBtn.SetActive(false);
        m_watchBtn.SetActive(false);
        m_countdownTextObj.SetActive(false);

        m_judgeText = m_judgeTextObj.GetComponent<Text>();
        m_countdownText = m_countdownTextObj.GetComponent<Text>();

    }

    void Update()
    {
        if (m_isCreateTank && m_tankPlayer == null)//photon側で死んだ判定の時の方がいいかも
        {
            Debug.Log("ゲームオーバー");
            m_gameOverPanel.SetActive(true);
            m_judgeTextObj.SetActive(true);
            m_exitBtn.SetActive(true);
            m_watchBtn.SetActive(true);

            m_judgeText.text = "Game Over";

            m_isCreateTank = false;
        }

        int totalPlayerNum = (PhotonNetwork.CurrentRoom.CustomProperties["alivePlayer"] is int value) ? value : 0;
        if (totalPlayerNum <= 1 && GameState.GetGameState() != GameState.e_GameState.Mactting)
        {
            int userId = (PhotonNetwork.LocalPlayer.CustomProperties["UserId"] is int _value) ? _value : 0;

            Debug.Log("Player " + userId + " is Win!!");
            m_gameOverPanel.SetActive(true);
            m_judgeTextObj.SetActive(true);
            m_exitBtn.SetActive(true);
            m_countdownTextObj.SetActive(true);

            m_judgeText.text = "Player " + userId + " is Win!!";

            m_isCreateTank = false;

            //5秒後にStartMenuへ移動
            m_countdown -= Time.deltaTime;
            m_countdownText.text = "タイトルに戻るまで　" + (int)m_countdown;
            if (m_countdown <= 0.0f)
            {
                PhotonNetwork.LeaveRoom();
                SceneManager.LoadSceneAsync("StartMenu", LoadSceneMode.Single);
            }
        }
    }

    public void CreateMyTank(GameObject tank, int playerID)
    {
        Debug.Log("Tankが生成された (PlayerID = " + playerID + ")");
        m_tankPlayer = tank;
        m_isCreateTank = true;
    }

    public void PushWatchButton()
    {
        m_gameOverPanel.SetActive(false);
        m_judgeTextObj.SetActive(false);
        m_exitBtn.SetActive(true);
        m_watchBtn.SetActive(false);
        m_countdownTextObj.SetActive(false);
    }

    public void PushExitButton()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadSceneAsync("StartMenu", LoadSceneMode.Single);
    }

}
