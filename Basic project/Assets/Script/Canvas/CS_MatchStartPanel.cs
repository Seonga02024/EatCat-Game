using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CS_MatchStartPanel : MonoBehaviour, PanelSetting
{
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject MTInfo;
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private TMP_Text infoText;
    [SerializeField] private TMP_Text waitText;
    [SerializeField] private Button backBtn;

    void Start()
    {
        backBtn.onClick.AddListener(OnBackBtnClicked);
    }

    private void OnBackBtnClicked()
    {
        CS_GameSoundManager.Instance.SfxPlay(SFX.Click_SFX);
        CS_MTGameManager.Instance.GameExit();
        Close();
    }

    public void ChangeTimeText(int num){
        timeText.text = num.ToString();
    }

    public void Init()
    {
        timeText.text = "5";
    }

    public void Open()
    {
        Init();
        mainPanel.SetActive(true);
    }

    public void Close()
    {
        mainPanel.SetActive(false);
    }
    
    public void OnWaitText(bool isActive, bool isMTGame){
        // 상대방 대기 text
        if(isActive){
            timeText.gameObject.SetActive(false);
            infoText.gameObject.SetActive(false);
            waitText.gameObject.SetActive(true);
            backBtn.gameObject.SetActive(true);
        }else{
            // 카운트 다운 text
            timeText.gameObject.SetActive(true);
            infoText.gameObject.SetActive(true);
            waitText.gameObject.SetActive(false);
            backBtn.gameObject.SetActive(false);

            if(isMTGame){
                infoText.text = "먼저 100점 점수내면 끝!";
            }else{
                infoText.text = "오랫동안 먹어라!";
            }
        }

        if(isMTGame){
            MTInfo.SetActive(true);
        }else{
            MTInfo.SetActive(false);
        }
    }
    
}
