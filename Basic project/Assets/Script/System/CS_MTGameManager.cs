using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class CS_MTGameManager : MonoBehaviourPunCallbacks, IPunObservable
{
    [Header("Game Object")]
    [SerializeField] private Transform foodStartPoint;
    [SerializeField] private Transform foodEndPoint;
    [SerializeField] private CS_FoodCheck foodCheckCS;
    private CS_Player myPlayerCS;
    public CS_Player MyPlayerCS { set { myPlayerCS = value; } }

    [Header("food Prefab")]
    [SerializeField] private GameObject[] foodPrefab;

    /* Game Values */
    private int currentFoodMaxRangeNum = 2;
    private int currentCorrectFoodIndex = 0;
    public int GetCurrentCorrectFoodIndex() { return currentCorrectFoodIndex; }
    private int currentMyScore = 0;
    private int currentOtherScore = 0;
    private bool isMTGameStart = false;
    public bool IsMTGameStart { get { return isMTGameStart; } }

    /* Network Values */
    public PhotonView PV;
    private bool isHost = false;
    public bool IsHost { get { return isHost; } }

    /* Coroutine */
    private Coroutine foodCreteCoroutine = null;

    /* WaitForSeconds */
    private WaitForSeconds waitFor4Seconds= new WaitForSeconds(4);

    /// ////////////////////////////////////////////////////////////////////////////////////////////////
    /// connect sever and start game  + setting host

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log(newPlayer.NickName + " joined the room.");
        CheckStartGame();
    }

    public override void OnJoinedRoom(){
        // if (PhotonNetwork.CurrentRoom.PlayerCount == 1){
        //     Debug.Log("I am host");
        //     isHost = true;
        // }
        isHost = PhotonNetwork.IsMasterClient;

        if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            Debug.Log("Room is full. Starting game...");
            StartGame();
        }
    }

    private void CheckStartGame()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            Debug.Log("Room is full. Starting game...");
            StartGame();
        }
    }

    /// ////////////////////////////////////////////////////////////////////////////////////////////////
    /// game logic

    private void StartGame()
    {
        // 게임 시작 로직을 여기에 추가하세요.
        // 예를 들어, 게임 씬으로 전환하거나 게임 오브젝트를 활성화합니다.
        CS_CanvasController.Instance.GetGamePanel().Init();
        Debug.Log("Game started!");
        ChangeCurrentCorrectFood();
        // 4초마다 반복 실행
        if(foodCreteCoroutine != null) StopCoroutine(foodCreteCoroutine);
        foodCreteCoroutine = StartCoroutine(CreateFoodAndMove());
        isMTGameStart = true;
    }

    private void ChangeCurrentCorrectFood()
    {
        if(isHost){
            int randomNum = Random.Range(0, currentFoodMaxRangeNum);
            currentCorrectFoodIndex = randomNum;
        }
    }

    IEnumerator CreateFoodAndMove()
    {
        while (true)
        {
            yield return waitFor4Seconds;
            if(isHost){
                int randomNum = Random.Range(0, currentFoodMaxRangeNum);
                PV.RPC("CurrentFoodIndex", RpcTarget.All, randomNum);
                Debug.Log("ReleaseAndAddFood");
            }
        }
    }

    [PunRPC]
    void CurrentFoodIndex(int index, PhotonMessageInfo info) => FoodCreateAndMove(index, info);
    [PunRPC]
    void CheckMoveEatFood(int index, PhotonMessageInfo info) => MoveEatFood(index, info);

    private void FoodCreateAndMove(int index, PhotonMessageInfo info){
        // if(info.Sender.IsLocal){
        //     CS_NetworkManager.Instance.testText.text = "My Get" + index;
        // }else{
        //     CS_NetworkManager.Instance.testText.text = "Other Get" + index;
        // }
        //GameObject food = Instantiate(foodPrefab[index], foodStartPoint);
        if(isHost){
            GameObject food = PhotonNetwork.Instantiate(foodPrefab[index].name, foodStartPoint.position, Quaternion.identity, 0);
        }
    }

    public void CheckEatCorrrectFood(){
        Debug.Log("CheckEatCorrrectFood");
        //if(foodCheckCS.CurrentFoodIndex >= 0){
            // 한동안 터치 못하게 막기
            photonView.RPC("CheckMoveEatFood", RpcTarget.All, 1);
        //}
    }

    private void MoveEatFood(int index, PhotonMessageInfo info){
        //Debug.Log("MoveEatFood");

        if(foodCheckCS.IscurrentGet == false){
            // 본인이 호스트면 점수 획득
            if (isHost){
                if(currentCorrectFoodIndex == foodCheckCS.CurrentFoodIndex){
                    // 점수 얻기
                    //PV.RPC("GetScore", RpcTarget.All, 2);
                    CS_NetworkManager.Instance.testText.text = "CorrectFood" + currentCorrectFoodIndex + " " + foodCheckCS.CurrentFoodIndex;
                    if(info.Sender.IsLocal){
                        currentMyScore += 10;
                    }else{
                        currentOtherScore += 10;
                    }
                }else{
                    CS_NetworkManager.Instance.testText.text = "UnCorrectFood" + currentCorrectFoodIndex + " " + foodCheckCS.CurrentFoodIndex;
                    if(info.Sender.IsLocal){
                        currentMyScore -= 10;
                        if(currentMyScore < 0) currentMyScore = 0;
                    }else{
                        currentOtherScore -= 10;
                        if(currentOtherScore < 0) currentOtherScore = 0;
                    }
                }
            }

            if(info.Sender.IsLocal){
                //CS_NetworkManager.Instance.testText.text = "My Get" + index;
                // 음식 움직이기
                foodCheckCS.MoveFoodToCat(true);
                myPlayerCS.playEatAnimation();
            }else{
                //CS_NetworkManager.Instance.testText.text = "Other Get" + index;
                // 음식 움직이기
                foodCheckCS.MoveFoodToCat(false);
            }
        }
    }

    // 점수를 업데이트하는 메서드 예제
    public void UpdateMyScore(int score)
    {
        currentMyScore = score;
        CS_CanvasController.Instance.GetGamePanel().ChangeMyScoreText(currentMyScore);
    }

    public void UpdateOtherScore(int score)
    {
        currentOtherScore = score;
        CS_CanvasController.Instance.GetGamePanel().ChangeOtherScoreText(currentOtherScore);
    }

    public void UpdateCurrentCorrectFoodImg(int index){
        currentCorrectFoodIndex = index;
        CS_CanvasController.Instance.GetGamePanel().ChangeCurrentCorrectFoodImg(currentCorrectFoodIndex);
    }   

    /// ////////////////////////////////////////////////////////////////////////////////////////////////
    /// sever
    

    // 변수 동기화 진행
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){
        if(stream.IsWriting){
            if(isHost){
                //Debug.Log("OnPhotonSerializeView send");
                stream.SendNext(currentMyScore);
                stream.SendNext(currentOtherScore);
                stream.SendNext(currentCorrectFoodIndex);
                UpdateMyScore(currentMyScore);
                UpdateOtherScore(currentOtherScore);
                UpdateCurrentCorrectFoodImg(currentCorrectFoodIndex);
            }
        }else{
            //Debug.Log("OnPhotonSerializeView get");
            UpdateOtherScore((int)stream.ReceiveNext());
            UpdateMyScore((int)stream.ReceiveNext());
            UpdateCurrentCorrectFoodImg((int)stream.ReceiveNext());
        }
    }

    /// ////////////////////////////////////////////////////////////////////////////////////////////////
    /// singleton

    private static CS_MTGameManager _instance;

    public static CS_MTGameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<CS_MTGameManager>();

                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject();
                    _instance = singletonObject.AddComponent<CS_MTGameManager>();
                    singletonObject.name = typeof(CS_MTGameManager).ToString() + " (Singleton)";

                    DontDestroyOnLoad(singletonObject);
                }
            }

            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this as CS_MTGameManager;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    /// ////////////////////////////////////////////////////////////////////////////////////////////////

}
