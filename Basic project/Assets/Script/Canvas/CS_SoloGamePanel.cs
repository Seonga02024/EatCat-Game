using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CS_SoloGamePanel : MonoBehaviour, PanelSetting
{
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private Image currentCorrectFoodImg1;
    [SerializeField] private Image currentCorrectFoodImg2;
    [SerializeField] private TMP_Text myScore;
    [SerializeField] private Sprite[] foodImg;
    [SerializeField] private Button rightClickBtn;
    [SerializeField] private Button leftClickBtn;
    [SerializeField] private GameObject[] heartsImg;
    private bool isEating = false;
    private int currentFoodImgIndex1 = -1;
    private int currentFoodImgIndex2 = -1;
    private int currentMyScore = 0;
    private const int heartMaxNumber = 5;
    private int currentHeartNum = 0;


    public void ChangeCurrentCorrectFoodImg(int index1, int index2 ){
        currentCorrectFoodImg1.sprite = foodImg[index1];
        if(currentFoodImgIndex1 != index1){
            CS_GameSoundManager.Instance.SfxPlay(SFX.ChangeFood_SFX);
            currentFoodImgIndex1 = index1;
        }

        currentCorrectFoodImg2.sprite = foodImg[index2];
        if(currentFoodImgIndex2 != index2){
            CS_GameSoundManager.Instance.SfxPlay(SFX.ChangeFood_SFX);
            currentFoodImgIndex2 = index2;
        }
    }
    
    public void ChangeMyScoreText(int score){
        myScore.text = score.ToString();
        if(currentMyScore != score){
            if(currentMyScore < score){
                CS_GameSoundManager.Instance.SfxPlay(SFX.GetPoint_SFX);
            }
            // else{
            //     CS_GameSoundManager.Instance.SfxPlay(SFX.LosePoint_SFX);
            // }
            currentMyScore = score;
        }
    }

    public void ChangeHeartImg(int heartNum){
        if(currentHeartNum > heartNum){
            CS_GameSoundManager.Instance.SfxPlay(SFX.LosePoint_SFX);
        }
        currentHeartNum = heartNum;
        int dieNum = heartMaxNumber - heartNum;
        for(int i = 0; i<heartMaxNumber; i++){
            if(i < dieNum){
                if(heartsImg[i].activeSelf == true){
                    heartsImg[i].SetActive(false);
                }
            }else{
                if(heartsImg[i].activeSelf == false){
                    heartsImg[i].SetActive(true);
                }
            }
        }
    }

    public void Init()
    {
        currentCorrectFoodImg1.sprite = null;
        currentCorrectFoodImg2.sprite = null;
        currentMyScore = 0;
        myScore.text = "0";
        ChangeHeartImg(heartMaxNumber);
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
        rightClickBtn.onClick.AddListener(OnRightClicked);
        leftClickBtn.onClick.AddListener(OnLeftClicked);
    }

    private void OnRightClicked()
    {
        CS_GameSoundManager.Instance.SfxPlay(SFX.Eat_SFX);
        if(CS_SoloGameManager.Instance.IsSoloGameStart){
            if(isEating == false){
                SendTouchData(false);
            }
        }
    }

    private void OnLeftClicked()
    {
        CS_GameSoundManager.Instance.SfxPlay(SFX.Eat_SFX);
        if(CS_SoloGameManager.Instance.IsSoloGameStart){
            if(isEating == false){
                SendTouchData(true);
            }
        }
    }

    private void SendTouchData(bool isLeft){
        CS_SoloGameManager.Instance.CheckEatCorrrectFood(isLeft);
        isEating = true;
        Invoke("WaitEating", 1);
    }

    private void WaitEating(){
        isEating = false;
    }
}
