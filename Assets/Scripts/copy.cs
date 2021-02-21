//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using Photon.Pun;

//public class ButtonController : MonoBehaviour
//{
//	[SerializeField]
//	private Photon_connect connect;
//	[SerializeField]
//	private GameObject MainTankGameObj;
//	[SerializeField]
//	private GameObject joystickCanvasObj;
//	[SerializeField]
//	private GameObject matchingCanvas;
//	[SerializeField]
//	private GameObject createButtonObj;
//	[SerializeField]
//	private GameObject joinButtonObj;
//	[SerializeField]
//	private GameObject roomNameInputFieldObj;
//	[SerializeField]
//	private InputField inputField;
//	[SerializeField]
//	private GameObject warningTextObj;
//	[SerializeField]
//	private GameObject cautionTextObj;
//	[SerializeField]
//	private GameObject randomRoomObj;
//	[SerializeField]
//	private GameObject undoButtonObj;

//	public string roomName;
//	public bool isCreate;
//	public bool isJoin;
//	public bool isRandomJoin;
//	public bool isGetRoomName;

//	private void Start()
//	{
//		Init();
//	}

//	private void Init()
//	{
//		LoadCanvas(false, true);
//		isCreate = false;
//		isJoin = false;
//		isGetRoomName = false;
//		isRandomJoin = false;
//		roomName = "";
//		inputField.text = "";
//		ActiveJoinOrCreateButtons(true);
//		roomNameInputFieldObj.SetActive(false);
//		warningTextObj.SetActive(false);
//		cautionTextObj.SetActive(false);
//		randomRoomObj.SetActive(false);
//		undoButtonObj.SetActive(false);
//	}

//	private void ActiveJoinOrCreateButtons(bool active)
//	{
//		createButtonObj.SetActive(active);
//		joinButtonObj.SetActive(active);
//	}

//	public void GetRoomName()
//	{
//		roomName = inputField.text;

//		if (roomName.Length > 10 || roomName.Length == 0)
//		{
//			warningTextObj.SetActive(true);
//			GetRoomName();
//		}
//		isGetRoomName = true;

//		if (isGetRoomName)
//		{
//			LoadCanvas(true, true);
//		}
//	}

//	public void PushCreateButton()
//	{
//		isCreate = true;
//		isJoin = false;
//		ActiveJoinOrCreateButtons(false);
//		roomNameInputFieldObj.SetActive(true);
//		cautionTextObj.SetActive(true);
//		undoButtonObj.SetActive(true);
//	}

//	public void PushJoinButton()
//	{
//		isJoin = true;
//		isCreate = false;
//		ActiveJoinOrCreateButtons(false);
//		roomNameInputFieldObj.SetActive(true);
//		cautionTextObj.SetActive(true);
//		randomRoomObj.SetActive(true);
//		undoButtonObj.SetActive(true);
//		Debug.Log("PushJoinButton");
//	}

//	public void PushRandomJoinButton()
//	{
//		isJoin = true;
//		isRandomJoin = true;
//		isCreate = false;

//		LoadCanvas(true, false);
//	}

//	public void PushUndoButton()
//	{
//		undoButtonObj.SetActive(false);
//		randomRoomObj.SetActive(false);
//		warningTextObj.SetActive(false);
//		cautionTextObj.SetActive(false);
//		roomNameInputFieldObj.SetActive(false);
//		isCreate = false;
//		isJoin = false;
//		isRandomJoin = false;
//		isGetRoomName = false;
//		ActiveJoinOrCreateButtons(true);
//	}

//	public void LoadCanvas(bool mainCanvasOn, bool matchingCanvasOn, bool joystickCanvasOn = false)
//	{
//		MainTankGameObj.SetActive(mainCanvasOn);
//		matchingCanvas.SetActive(matchingCanvasOn);
//		joystickCanvasObj.SetActive(joystickCanvasOn);
//	}

//	public void ReMatching()
//	{
//		if (connect.IsMakeRoomFailed)
//		{
//			Init();
//		}
//	}

//}


//public class Photon_connect : MonoBehaviour
//{
//	[SerializeField]
//	private ButtonController matchingController;
//	[SerializeField]
//	private string m_resourcePath = "";
//	[SerializeField]
//	private float m_randomCircle = 4.0f;
//	private string ROOM_NAME = "defaultRoom";
//	public bool AutoConnect = true;
//	public byte Version = 1;
//	private bool ConnectInUpdate = true;
//	private PhotonView m_photonView = null;
//	private Vector3[] startPos = { new Vector3(16, 0, 12), new Vector3(-16, 0, 12), new Vector3(-16, 0, -12), new Vector3(16, 0, -12) };

//	private bool isManualOnConnect = false;
//	public bool IsMakeRoomFailed { get; private set; }

//	// cube生成
//	public void SpawnObject()
//	{
//		matchingController.LoadCanvas(true, false, true);

//		GameObject player = PhotonNetwork.Instantiate("Player", new Vector3(0, 0, 0), Quaternion.identity, 0);
//		if (!player.GetComponent<Rigidbody>())
//		{
//			player.gameObject.AddComponent<Rigidbody>();
//		}

//		m_photonView = player.GetComponent<PhotonView>();
//		int ownerID = m_photonView.ownerId;
//		Vector3 playerPos = player.transform.position;
//		playerPos = startPos[(ownerID - 1) % 4];
//		player.transform.position = playerPos;
//	}

//	// 生成するcubeの座標をランダム生成
//	private Vector3 GetRandomPosition()
//	{
//		var rand = Random.insideUnitCircle * m_randomCircle;
//		return rand;
//	}

//	public virtual void Update()
//	{
//		// Debug.Log("RoomName: "+ROOM_NAME+ ", RandomJoin: "+isRandomJoin+ ", IsGetName: "+isGetRoomName);

//		if (ConnectInUpdate && AutoConnect && !PhotonNetwork.connected)
//		{
//			Debug.Log("Update() was called by Unity. Scene is loaded. Let's connect to the Photon Master Server. Calling: PhotonNetwork.ConnectUsingSettings();");
//			ConnectInUpdate = false;
//			PhotonNetwork.ConnectUsingSettings(Version + "." + SceneManagerHelper.ActiveSceneBuildIndex);
//		}
//		if (isManualOnConnect)
//		{
//			OnConnectedToMaster();
//			isManualOnConnect = false;
//		}
//		//if (Input.GetMouseButton(0))
//		//{
//		//    SpawnObject();
//		//}
//	}

//	public virtual void OnConnectedToMaster()
//	{
//		Debug.Log("OnConnectedToMaster() was called by PUN. Now this client is connected and could join a room. Calling: PhotonNetwork.JoinRandomRoom();");
//		//PhotonNetwork.JoinRandomRoom();
//		//PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = 4 }, null);
//		// PhotonNetwork.JoinOrCreateRoom(ROOM_NAME, new RoomOptions(), TypedLobby.Default);
//		Debug.Log("WhenConnect...RoomName: " + matchingController.roomName + ", RandomJoin: " + matchingController.isRandomJoin + ", IsGetName: " + matchingController.isGetRoomName + ", isJoin: " + matchingController.isJoin + ", isCreate: " + matchingController.isCreate);

//		if (matchingController.isGetRoomName || matchingController.isRandomJoin)
//		{
//			if (matchingController.isRandomJoin)
//			{
//				PhotonNetwork.JoinRandomRoom();
//				Debug.Log("RandomRoomName" + ROOM_NAME);
//			}
//			else if (matchingController.isJoin)
//			{
//				// join失敗したらofflineになるかも?
//				PhotonNetwork.JoinOrCreateRoom(ROOM_NAME, new RoomOptions() { MaxPlayers = 4 }, TypedLobby.Default);
//				Debug.Log("JoinRoomCOmplete");
//			}
//			else
//			{
//				PhotonNetwork.CreateRoom(ROOM_NAME, new RoomOptions(), TypedLobby.Default);
//				Debug.Log("CreateRoomComplete");
//			}
//		}
//	}

//	public void OnJoinedRoom()
//	{
//		Debug.Log("OnJoinedRoom() called by PUN. Now this client is in a room. From here on, your game would be running. For reference, all callbacks are listed in enum: PhotonNetworkingMessage");
//		SpawnObject();
//	}

//	public void OnPhotonCreateRoomFailed()
//	{
//		// ルーム作成失敗時
//		Debug.Log("OnCreateRoom() called by PUN. But Failed so RoomName is exist.");
//		IsMakeRoomFailed = true;
//		matchingController.ReMatching();
//		isManualOnConnect = true;
//	}
//}