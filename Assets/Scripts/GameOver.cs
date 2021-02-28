using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameOver : MonoBehaviour
{
    [SerializeField] private GameObject m_tankPlayer;
    [SerializeField] private GameObject m_gameOverPanel;
    private bool m_isCreateTank = false;
        
    void Awake()
    {
        m_gameOverPanel.SetActive(false);
    }

    void Update()
    {
        if(m_isCreateTank && m_tankPlayer == null)//photon側で死んだ判定の時の方がいいかも
        {
            Debug.Log("ゲームオーバー");
            m_gameOverPanel.SetActive(true);
            m_isCreateTank = false;
        }
    }

    public void CreateMyTank(GameObject tank, int playerID)
    {
        Debug.Log("Tankが生成された (PlayerID = " + playerID + ")");
        m_tankPlayer = tank;
        m_isCreateTank = true;
    }
}
