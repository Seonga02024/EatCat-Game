using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CS_GamePanel : MonoBehaviour, PanelSetting
{
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private Image currentCorrectFoodImg;
    [SerializeField] private TMP_Text otherScore;
    [SerializeField] private TMP_Text myScore;
    [SerializeField] private Sprite[] foodImg;
    [SerializeField] private Button clickBtn;
    private bool isEating = false;
    private int currentFoodImgIndex = -1;
    private int currentMyScore = 0;
    
    [Header("Debug/Test Log")]
    [SerializeField] private TMP_Text hostText;
    [SerializeField] private TMP_Text gameInfoText;

    public void ChangeHostText(bool isHost){
        if(isHost) hostText.text = "Host";
        else hostText.text = "No Host";
    }
    
    public void ChangeGameInfoText(float waitTime, int foodSpeed){
        gameInfoText.text = "wait time : " + waitTime.ToString() + "\n" + "food speed : " + foodSpeed.ToString();
    }

    public void ChangeCurrentCorrectFoodImg(int index){
        currentCorrectFoodImg.sprite = foodImg[index];
        if(currentFoodImgIndex != index){
            CS_GameSoundManager.Instance.SfxPlay(SFX.ChangeFood_SFX);
            currentFoodImgIndex = index;
        }
    }
    
    public void ChangeMyScoreText(int score){
        myScore.text = score.ToString();
        if(currentMyScore != score){
            if(currentMyScore < score){
                CS_GameSoundManager.Instance.SfxPlay(SFX.GetPoint_SFX);
            }else{
                CS_GameSoundManager.Instance.SfxPlay(SFX.LosePoint_SFX);
            }
            currentMyScore = score;
        }
    }

    public void ChangeOtherScoreText(int score){
        otherScore.text = score.ToString();
    }

    public void Init()
    {
        currentCorrectFoodImg.sprite = null;
        otherScore.text = "0";
        myScore.text = "0";
    }

    public void Open()
    {
        mainPanel.SetActive(true);
    }

    public void Close()
    {
        mainPanel.SetActive(false);
    }

    void Start()
    {
        clickBtn.onClick.AddListener(OnClicked);
    }

    private void OnClicked()
    {
        CS_GameSoundManager.Instance.SfxPlay(SFX.Eat_SFX);
        if(CS_MTGameManager.Instance.IsMTGameStart){
            if(isEating == false){
                SendTouchData(true);
            }
        }
    }

    private void SendTouchData(bool isLeft){
        // if(CS_GameManager.Instance.IsGameStart){

        // }else{
            CS_MTGameManager.Instance.CheckEatCorrrectFood();
            isEating = true;
            Invoke("WaitEating", 2);
        //}
    }

    private void WaitEating(){
        isEating = false;
    }
}
