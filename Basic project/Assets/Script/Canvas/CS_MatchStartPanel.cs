using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CS_MatchStartPanel : MonoBehaviour, PanelSetting
{
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private TMP_Text timeText;

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
}
