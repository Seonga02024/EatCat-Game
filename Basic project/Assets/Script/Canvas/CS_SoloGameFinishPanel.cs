using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CS_SoloGameFinishPanel : MonoBehaviour
{
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private TMP_Text myHighScore;
    [SerializeField] private TMP_Text myScore;
    [SerializeField] private TMP_Text coin;
    [SerializeField] private Button backBtn;
    [SerializeField] private Button ADBtn;

    void Start()
    {
        Init();
    }

    public void Setting(int myscore, int coinNum)
    {
        CS_SaveLoadManager.Instance.SetHighScore(myscore);
        CS_GameSoundManager.Instance.SfxPlay(SFX.WinResult_SFX);
        coin.text = coinNum.ToString();
        myHighScore.text = "내 최고 점수 : " + CS_SaveLoadManager.Instance.GetHighScore().ToString();
        myScore.text = myscore.ToString();
        CS_SaveLoadManager.Instance.SaveData();
    }

    public void Init()
    {
        backBtn.onClick.AddListener(OnBackBtnClicked);
        ADBtn.onClick.AddListener(OnADBtnClicked);
    }

    private void OnBackBtnClicked()
    {
        Debug.Log("Back Button Clicked");
        CS_SoloGameManager.Instance.GameExit();
        CS_GameSoundManager.Instance.SfxPlay(SFX.Click_SFX);
        CS_GameSoundManager.Instance.BgmSet(BGM.Main_BGM);
        CS_GameSoundManager.Instance.BgmPlay();
        Close();
    }

    private void OnADBtnClicked()
    {
        Debug.Log("AD Button Clicked");
        CS_GameSoundManager.Instance.SfxPlay(SFX.Click_SFX);
    }

    public void Open()
    {
        mainPanel.SetActive(true);
    }

    public void Close()
    {
        mainPanel.SetActive(false);
        CS_CanvasController.Instance.GetSoloGamePanel().Close();
    }
}
