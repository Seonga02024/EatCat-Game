using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public class MultiplayerGameState : MonoBehaviourPunCallbacks, IGameState, IPunObservable
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
    private int gameTimer = 0;
    private float currentWaitTime;
    private float initialWaitTime = 4.0f;
    private float minimumWaitTime = 1.0f;
    private int nextStepWaitTime = 10;
    private float decrementWaitTimeStep = 0.3f;
    private int currentFoodMoveSpeed;
    private int maxmumFoodMoveSpeed = 200;
    private int initialFoodMoveSpeed = 100;
    private int increaseFoodMoveSpeedStep = 10;
    private int nextStepChangeCorrectFoodTime = 20;

    /* Network Values */
    public PhotonView PV;
    private bool isHost = false;
    public bool IsHost { get { return isHost; } }

    /* Coroutine */
    private Coroutine foodCreteCoroutine = null;
    private Coroutine waitStartCoroutine = null;
    private Coroutine gameTimeCoroutine = null;

    /* WaitForSeconds */
    private WaitForSeconds waitFor1Seconds= new WaitForSeconds(1);

    /// ////////////////////////////////////////////////////////////////////////////////////////////////
    
    private GameManager _gameManager;

    public MultiplayerGameState(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    /// ////////////////////////////////////////////////////////////////////////////////////////////////

    public void EnterState()
    {
        Debug.Log("Entered Multiplayer Game State");
        // 멀티 게임 모드 초기화 로직을 수행합니다.
    }

    public void UpdateState()
    {
        // 멀티 게임 모드에서 필요한 업데이트 로직을 수행합니다.
    }

    public void ExitState()
    {
        Debug.Log("Exiting Multiplayer Game State");
        // 멀티 게임 모드를 벗어날 때 필요한 정리 작업을 수행합니다.
    }

    /// ////////////////////////////////////////////////////////////////////////////////////////////////
    /// EnterState

    // ////////////////////////////////////////////////////////////////////////////////////////////////
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
        CS_CanvasController.Instance.GetGamePanel().ChangeHostText(isHost);

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
        Debug.Log("Game started!");

        // 게임 시작 로직을 여기에 추가하세요.
        // 예를 들어, 게임 씬으로 전환하거나 게임 오브젝트를 활성화합니다.
        CS_CanvasController.Instance.GetGamePanel().Init();
        CS_CanvasController.Instance.GetMatchStartPanel().Open();

        ChangeCurrentCorrectFood();

        currentWaitTime = initialWaitTime;
        currentFoodMoveSpeed = initialFoodMoveSpeed;
        CS_CanvasController.Instance.GetGamePanel().ChangeGameInfoText(currentWaitTime, currentFoodMoveSpeed);

        if(gameTimeCoroutine != null) StopCoroutine(gameTimeCoroutine);
        if(foodCreteCoroutine != null) StopCoroutine(foodCreteCoroutine);
        if(waitStartCoroutine != null) StopCoroutine(waitStartCoroutine);
        waitStartCoroutine = StartCoroutine(WaitStartGame());
    }

    private void ChangeCurrentCorrectFood()
    {
        if(isHost){
            int randomNum = Random.Range(0, currentFoodMaxRangeNum);
            currentCorrectFoodIndex = randomNum;
        }
    }

    IEnumerator WaitStartGame()
    {
        int waitTime = 5;
        while (waitTime > 0)
        {
            yield return waitFor1Seconds;
            waitTime -= 1;
            CS_CanvasController.Instance.GetMatchStartPanel().ChangeTimeText(waitTime);
        }

        CS_CanvasController.Instance.GetMatchStartPanel().Close();

        if(foodCreteCoroutine != null) StopCoroutine(foodCreteCoroutine);
        foodCreteCoroutine = StartCoroutine(CreateFoodAndMove());

        if(gameTimeCoroutine != null) StopCoroutine(gameTimeCoroutine);
        gameTimeCoroutine = StartCoroutine(GameTimer());

        isMTGameStart = true;
    }

    IEnumerator CreateFoodAndMove()
    {
        while (true)
        {
            if(isHost){
                int randomNum = Random.Range(0, currentFoodMaxRangeNum);
                PV.RPC("CurrentFoodIndex", RpcTarget.All, randomNum);
                Debug.Log("ReleaseAndAddFood");
            }
            yield return new WaitForSeconds(currentWaitTime);
        }
    }

    IEnumerator GameTimer()
    {
        while (true)
        {
            yield return waitFor1Seconds;
            gameTimer += 1;

            if(gameTimer%nextStepWaitTime == 0){
                if(currentWaitTime - decrementWaitTimeStep >= minimumWaitTime) currentWaitTime -= decrementWaitTimeStep;
                if(currentFoodMoveSpeed + increaseFoodMoveSpeedStep <= maxmumFoodMoveSpeed) currentFoodMoveSpeed += increaseFoodMoveSpeedStep;
                CS_CanvasController.Instance.GetGamePanel().ChangeGameInfoText(currentWaitTime, currentFoodMoveSpeed);
            }

            if(gameTimer%nextStepChangeCorrectFoodTime == 0){
                ChangeCurrentCorrectFood();
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
            food.GetComponent<CS_moveFood>().ChangeMoveSpeed(currentFoodMoveSpeed);
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
}
