using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CS_GameFinishPanel : MonoBehaviour, PanelSetting
{
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private TMP_Text otherScore;
    [SerializeField] private TMP_Text myScore;
    [SerializeField] private TMP_Text coin;
    [SerializeField] private Button backBtn;

    void Start()
    {
        Init();
    }

    public void Setting(int myscore, int otherscore, int coinNum)
    {
        myScore.text = myscore.ToString();
        otherScore.text = otherscore.ToString();
        if(myscore > otherscore){
            CS_GameSoundManager.Instance.SfxPlay(SFX.WinResult_SFX);
            coin.text = coinNum.ToString();
        }else{
            CS_GameSoundManager.Instance.SfxPlay(SFX.WinResult_SFX);
            coin.text = "0";
        }
    }

    public void Init()
    {
        backBtn.onClick.AddListener(OnBackBtnClicked);
    }

    private void OnBackBtnClicked()
    {
        Debug.Log("Back Button Clicked");
        CS_MTGameManager.Instance.GameExit();
        CS_GameSoundManager.Instance.SfxPlay(SFX.Click_SFX);
        CS_GameSoundManager.Instance.BgmSet(BGM.Main_BGM);
        CS_GameSoundManager.Instance.BgmPlay();
        Close();
    }

    public void Open()
    {
        mainPanel.SetActive(true);
    }

    public void Close()
    {
        mainPanel.SetActive(false);
        CS_CanvasController.Instance.GetGamePanel().Close();
    }
}
