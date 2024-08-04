using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_MatchPanel : MonoBehaviour, PanelSetting
{
    [SerializeField] private GameObject mainPanel;

    public void Init()
    {
    }

    public void Open()
    {
        mainPanel.SetActive(true);
    }

    public void Close()
    {
        mainPanel.SetActive(false);
    }
}
