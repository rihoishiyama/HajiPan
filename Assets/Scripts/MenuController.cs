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
    private GameObject undoBtnObj;
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
        createBtnObj.SetActive(true);
        undoBtnObj.SetActive(true);
        status = MenuStatus.Start;
    }

    public void JoinButton()
    {
        joinBtnObj.SetActive(false);
        createBtnObj.SetActive(false);

        selectJoinBtnObj.SetActive(true);
        randomJoinBtnObj.SetActive(true);
        undoBtnObj.SetActive(true);
        status = MenuStatus.Join;
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
}
