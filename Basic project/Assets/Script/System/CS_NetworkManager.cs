using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class CS_NetworkManager : MonoBehaviourPunCallbacks
{
    [SerializeField, Tooltip("This is my player nickName")]
    private TMP_InputField NickNameInput;
    [SerializeField] private GameObject DisconnectPanel;
    [SerializeField] private GameObject RespawnPanel;
    [SerializeField] public TMP_Text testText;

    void Awake(){
        //Screen.SetResolution(1080, 1920, false);
        // 아래 두 개를 설정해주면 동기화가 더 빨리 된다고 함
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 30;
        PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = "us"; // 예: us, eu 등

        if (_instance == null)
        {
            _instance = this as CS_NetworkManager;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Connet() => PhotonNetwork.ConnectUsingSettings(); // 서버 바로 연결 시도

    public override void OnConnectedToMaster(){
        CS_GameSoundManager.Instance.SfxPlay(SFX.Click_SFX);
        Debug.Log("OnConnectedToMaster");
        // 버튼을 누르면 서버 연결해주는 함수
        // player 세팅 이후 Room 을 참여하거나 만들기
        PhotonNetwork.LocalPlayer.NickName = NickNameInput.text;
        if(NickNameInput.text == "") PhotonNetwork.LocalPlayer.NickName = "user";
        //PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 3 }, null);
        PhotonNetwork.JoinLobby(); // 방 목록을 가져와 방 이름을 확인
        //PhotonNetwork.JoinRandomRoom(); // 랜덤으로 방에 참여
    }

    // public override void OnJoinRandomFailed(short returnCode, string message) {
    //     // 랜덤으로 방에 참여 실패시 만들기
    //     PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 2 }, null);
    // }

    public override void OnRoomListUpdate(List<RoomInfo> roomList) {
        bool foundRoom = false;

        // 방 목록을 순회하여 빈 방을 찾기
        foreach (RoomInfo room in roomList) {
            Debug.Log("Room Name: " + room.Name + " - Players: " + room.PlayerCount + "/" + room.MaxPlayers);

            // 방이 비어있는 경우
            if (room.PlayerCount < room.MaxPlayers) {
                if (room.CustomProperties.ContainsKey("gameStarted") && (bool)room.CustomProperties["gameStarted"] == false){
                    foundRoom = true;
                    PhotonNetwork.JoinRoom(room.Name);  // 빈 방에 입장
                    return;
                }
            }
        }

        // 빈 방이 없으면 새로운 방 생성
        if (!foundRoom) {
            string roomName = "Room_" + System.DateTime.Now.Ticks;  // 고유한 방 이름 생성
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = 2;
            roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable { { "gameStarted", false } };
            roomOptions.CustomRoomPropertiesForLobby = new string[] { "gameStarted" };
            PhotonNetwork.CreateRoom(roomName, roomOptions);
        }
    }

    public override void OnJoinedRoom(){
        Debug.Log("OnJoinedRoom");

        // 현재 방의 이름 출력
        if (PhotonNetwork.CurrentRoom != null) {
            Debug.Log("Joined Room Name: " + PhotonNetwork.CurrentRoom.Name);
            testText.text = "Joined Room Name: " + PhotonNetwork.CurrentRoom.Name;
        }

        // 방에 참여하게 되면 호출되는 함수
        //DisconnectPanel?.SetActive(false);
        Spawn();
    }

    public void Spawn()
    {
        Debug.Log("Spawn");
        if(PhotonNetwork.IsMasterClient){
            GameObject player = PhotonNetwork.Instantiate("Player1", Vector3.zero, Quaternion.identity);
        }else{
            GameObject player = PhotonNetwork.Instantiate("Player2", Vector3.zero, Quaternion.identity);
        }
        //RespawnPanel?.SetActive(false);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && PhotonNetwork.IsConnected){
            // 서버 끊는 방법
            PhotonNetwork.Disconnect();
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("OnDisconnected");
        // 서버 연결이 끊어 졌을 때 호출되는 함수
        //DisconnectPanel?.SetActive(true);
        //RespawnPanel?.SetActive(false);
    }

    private static CS_NetworkManager _instance;

    public static CS_NetworkManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<CS_NetworkManager>();

                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject();
                    _instance = singletonObject.AddComponent<CS_NetworkManager>();
                    singletonObject.name = typeof(CS_NetworkManager).ToString() + " (Singleton)";

                    DontDestroyOnLoad(singletonObject);
                }
            }

            return _instance;
        }
    }

}
