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
        coin.text = coinNum.ToString();
    }

    public void Init()
    {
        backBtn.onClick.AddListener(OnBackBtnClicked);
    }

    private void OnBackBtnClicked()
    {
        Debug.Log("Back Button Clicked");
        CS_MTGameManager.Instance.GameExit();
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
