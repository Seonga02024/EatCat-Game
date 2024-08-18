using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_SoloGameManager : SingleTon<CS_SoloGameManager>
{
    [Header("Game Object")]
    [SerializeField] private Transform foodStartPoint;
    [SerializeField] private Transform foodEndPoint;
    [SerializeField] private CS_FoodCheck foodCheckCS;
    [SerializeField] private CS_Player myPlayer1CS;
    public CS_Player MyPlayer1CS { set { myPlayer1CS = value; } }
    [SerializeField] private CS_Player myPlayer2CS;
    public CS_Player MyPlayer2CS { set { myPlayer2CS = value; } }

    [Header("food Prefab")]
    [SerializeField] private GameObject[] foodPrefab;

    /* Game Values */
    private int currentFoodMaxRangeNum = 4;
    private int currentCorrectFoodIndex1 = 0;
    private int currentCorrectFoodIndex2 = 0;
    public int GetCurrentCorrectFoodIndex1() { return currentCorrectFoodIndex1; }
    public int GetCurrentCorrectFoodIndex2() { return currentCorrectFoodIndex2; }
    private int currentMyScore = 0;
    private bool isSoloGameStart = false;
    public bool IsSoloGameStart { get { return isSoloGameStart; } }
    private int gameTimer = 0;
    private float currentWaitTime = 0;
    const float initialWaitTime = 2.0f;
    const float minimumWaitTime = 1.0f;
    const int nextStepWaitTime = 10;
    const float decrementWaitTimeStep = 0.3f;
    private int currentFoodMoveSpeed = 0;
    public int CurrentFoodMoveSpeed { get { return currentFoodMoveSpeed; } }
    const int maxmumFoodMoveSpeed = 200;
    const int initialFoodMoveSpeed = 150;
    const int increaseFoodMoveSpeedStep = 10;
    const int nextStepChangeCorrectFoodTime = 20;
    private List<GameObject> foodList = new List<GameObject>();
    private List<CS_moveFood> foodCSList = new List<CS_moveFood>();
    private const int heartMaxNumber = 5;
    private int currentHeartNum = 0;

    /* Coroutine */
    private Coroutine foodCreteCoroutine = null;
    private Coroutine waitStartCoroutine = null;
    private Coroutine gameTimeCoroutine = null;

    /* WaitForSeconds */
    private WaitForSeconds waitFor1Seconds= new WaitForSeconds(1);

    /// ////////////////////////////////////////////////////////////////////////////////////////////////
    /// game logic
    
    public void GameEnter(){
        CS_CanvasController.Instance.GetSoloGamePanel().Open();
        StartGame();
    }

    private void StartGame()
    {
        Debug.Log("Game started!");

        // 게임 시작 로직을 여기에 추가하세요.
        // 예를 들어, 게임 씬으로 전환하거나 게임 오브젝트를 활성화합니다.
        CS_CanvasController.Instance.GetSoloGamePanel().Init();
        CS_CanvasController.Instance.GetMatchStartPanel().OnWaitText(false, false);
        CS_CanvasController.Instance.GetMatchStartPanel().Open();

        ChangeCurrentCorrectFood();

        currentWaitTime = initialWaitTime;
        currentFoodMoveSpeed = initialFoodMoveSpeed;
        currentHeartNum = heartMaxNumber;
        //CS_CanvasController.Instance.GetGamePanel().ChangeGameInfoText(currentWaitTime, currentFoodMoveSpeed);

        if(gameTimeCoroutine != null) StopCoroutine(gameTimeCoroutine);
        if(foodCreteCoroutine != null) StopCoroutine(foodCreteCoroutine);
        if(waitStartCoroutine != null) StopCoroutine(waitStartCoroutine);
        waitStartCoroutine = StartCoroutine(WaitStartGame());
    }

    private void ChangeCurrentCorrectFood()
    {
        int randomNum1 = Random.Range(0, currentFoodMaxRangeNum);
        currentCorrectFoodIndex1 = randomNum1;

        int randomNum2;
        do
        {
            randomNum2 = Random.Range(0, currentFoodMaxRangeNum);
        } while (randomNum2 == randomNum1);
        currentCorrectFoodIndex2 = randomNum2;

        CS_CanvasController.Instance.GetSoloGamePanel().ChangeCurrentCorrectFoodImg(currentCorrectFoodIndex1, currentCorrectFoodIndex2);
    }

    IEnumerator WaitStartGame()
    {
        CS_GameSoundManager.Instance.SfxPlay(SFX.CountDown_SFX);
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

        isSoloGameStart = true;
    }

    IEnumerator CreateFoodAndMove()
    {
        while (true)
        {
            int randomNum = Random.Range(0, currentFoodMaxRangeNum);
            GameObject food = Instantiate(foodPrefab[randomNum], foodStartPoint.position, Quaternion.identity);
            foodList.Add(food);
            foodCSList.Add(food.GetComponent<CS_moveFood>());
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


    public void CheckEatCorrrectFood(bool isLeft){
        //Debug.Log("CheckEatCorrrectFood : " + isLeft);
        if(foodCheckCS.CurrentFoodIndex >= 0){
            // 한동안 터치 못하게 막기
            if(foodCheckCS.IscurrentGet == false){
                if(isLeft){
                    if(currentCorrectFoodIndex1 == foodCheckCS.CurrentFoodIndex){
                        // 점수 얻기
                        CS_NetworkManager.Instance.testText.text = "CorrectFood" + currentCorrectFoodIndex1 + " " + foodCheckCS.CurrentFoodIndex;
                        currentMyScore += 10;
                    }else{
                        CS_NetworkManager.Instance.testText.text = "UnCorrectFood" + currentCorrectFoodIndex1 + " " + foodCheckCS.CurrentFoodIndex;
                        currentMyScore -= 10;
                        if(currentMyScore < 0) currentMyScore = 0;
                        currentHeartNum -= 1;
                        CS_CanvasController.Instance.GetSoloGamePanel().ChangeHeartImg(currentHeartNum);
                    }
                    foodCheckCS.MoveFoodToCat(true, true);
                }else{
                    if(currentCorrectFoodIndex2 == foodCheckCS.CurrentFoodIndex){
                        // 점수 얻기
                        CS_NetworkManager.Instance.testText.text = "CorrectFood" + currentCorrectFoodIndex2 + " " + foodCheckCS.CurrentFoodIndex;
                        currentMyScore += 10;
                    }else{
                        CS_NetworkManager.Instance.testText.text = "UnCorrectFood" + currentCorrectFoodIndex2 + " " + foodCheckCS.CurrentFoodIndex;
                        currentMyScore -= 10;
                        if(currentMyScore < 0) currentMyScore = 0;
                        currentHeartNum -= 1;
                        CS_CanvasController.Instance.GetSoloGamePanel().ChangeHeartImg(currentHeartNum);
                    }
                    foodCheckCS.MoveFoodToCat(true, false);
                }
                UpdateMyScore(currentMyScore);
            }
        }

        if(isLeft){
            myPlayer1CS.playEatAnimation();
        }else{
            myPlayer2CS.playEatAnimation();
        }

        if(currentHeartNum == 0){
            EndGame();
        }
    }

    public void UpdateMyScore(int score)
    {
        currentMyScore = score;
        CS_CanvasController.Instance.GetSoloGamePanel().ChangeMyScoreText(currentMyScore);
    }

    public void RemoveFood(GameObject food)
    {
        if (foodList.Contains(food))
        {
            foodList.Remove(food);
            Debug.Log("Removed Food: " + food.name);
        }
    }

    private void EndGame()
    {
        isSoloGameStart = false;
        CS_CanvasController.Instance.GetSoloGameFinishPanel().Setting(currentMyScore, 10000, 1000);
        CS_CanvasController.Instance.GetSoloGameFinishPanel().Open();

        if(gameTimeCoroutine != null) StopCoroutine(gameTimeCoroutine);
        if(foodCreteCoroutine != null) StopCoroutine(foodCreteCoroutine);
        if(waitStartCoroutine != null) StopCoroutine(waitStartCoroutine);
    }

    public void GameExit()
    {
        CS_CanvasController.Instance.GetSoloGamePanel().Close();
        CS_CanvasController.Instance.GetSoloGamePanel().Init();

        // reset
        currentCorrectFoodIndex1 = 0;
        currentCorrectFoodIndex2 = 0;
        currentMyScore = 0;
        isSoloGameStart = false;
        gameTimer = 0;
        currentWaitTime = initialWaitTime;
        currentFoodMoveSpeed = initialFoodMoveSpeed;
        currentHeartNum = heartMaxNumber;

        //foodCSList.ForEach(foodCS => foodCS.RemoveObject());
        foreach (GameObject food in foodList)
        {
            if (food != null)
            {
                Destroy(food);
            }
        }
        foodList.Clear();
        foodCSList.Clear();
    }
    
}
