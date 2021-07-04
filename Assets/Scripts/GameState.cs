using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    /// <summary>
    /// stateをセット : GameState.SetGameState(GameState.e_GameState.GameStart);
    /// stateをゲット : GameState.GetGameState();　
    /// </summary>

    private static GameState instance = null;

    // インスタンスのプロパティーは、実体が存在しないとき（＝初回参照時）実体を探して登録する
    public static GameState Instance => instance
        ?? (instance = GameObject.Find("GameState").GetComponent<GameState>());

    // 各ステータス
    public enum e_GameState
    {
        Mactting, //マッチング中
        GameStart,
        Game, //ゲーム中
        GameEnd, //ゲーム終了
    }
    public static e_GameState m_gameState;

    private void Awake()
    {
        // もしインスタンスが複数存在するなら、自らを破棄する
        if (this != Instance)
        {
            Destroy(this.gameObject);
            return;
        }

        // 唯一のインスタンスなら、シーン遷移しても残す
        DontDestroyOnLoad(this.gameObject);
    }

    private void OnDestroy()
    {
        // 破棄時に、登録した実体の解除を行う
        if (this == Instance) instance = null;
    }

    public static void SetGameState(e_GameState state)
    {
        m_gameState = state;
    }

    public static e_GameState GetGameState()
    {
        return m_gameState;
    }

    // Inspectorへ表示用
    [SerializeField] private e_GameState m_displayGameState;
    private void Update()
    {
        m_displayGameState = m_gameState;
    }
}
