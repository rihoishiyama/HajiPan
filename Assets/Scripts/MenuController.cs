using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
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
    private GameObject inputFieldObj;
    [SerializeField]
    private InputField inputRoomName;

    private string roomName;

    private void Start()
    {
        StartInit();
    }

    public void StartInit()
    {
        startBtnObj.SetActive(true);
        joinBtnObj.SetActive(false);
        createBtnObj.SetActive(false);
        selectJoinBtnObj.SetActive(false);
        randomJoinBtnObj.SetActive(false);
        inputFieldObj.SetActive(false);
    }

    public void StartButton()
    {
        Debug.Log("push startButton");
        startBtnObj.SetActive(false);
        joinBtnObj.SetActive(true);
        createBtnObj.SetActive(true);
    }

    public void JoinButton()
    {
        joinBtnObj.SetActive(false);
        createBtnObj.SetActive(false);

        selectJoinBtnObj.SetActive(true);
        randomJoinBtnObj.SetActive(true);
    }

    public void SelectJoinButton()
    {
        selectJoinBtnObj.SetActive(false);
        randomJoinBtnObj.SetActive(false);

        inputFieldObj.SetActive(true);
    }

    public void RandomJoinButton()
    {
        selectJoinBtnObj.SetActive(false);
        randomJoinBtnObj.SetActive(false);
    }

    public void CreateButton()
    {
        joinBtnObj.SetActive(false);
        createBtnObj.SetActive(false);

        inputFieldObj.SetActive(true);
    }

    public void EnterButton()
    {
        roomName = inputRoomName.text;
        Debug.Log(roomName);
    }
}
