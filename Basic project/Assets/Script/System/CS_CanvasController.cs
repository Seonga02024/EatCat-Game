using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_CanvasController : SingleTon<CS_CanvasController>
{
    [SerializeField]
    public GameObject gamePanel;
    private CS_GamePanel gamePanelCS;

    // [SerializeField]
    // public GameObject touchPanel;
    // private CS_TouchPanel touchPanelCS;

    [SerializeField]
    public GameObject matchStartPanel;
    private CS_MatchStartPanel matchStartPanelCS;

    [SerializeField]
    public GameObject matchPanel;
    private CS_MatchPanel matchPanelCS;

    [SerializeField]
    public GameObject gameFinishPanel;
    private CS_GameFinishPanel gameFinishPanelCS;

    void Start()
    {
        gamePanelCS = gamePanel.GetComponent<CS_GamePanel>();
        //touchPanelCS = touchPanel.GetComponent<CS_TouchPanel>();
        matchStartPanelCS = matchStartPanel.GetComponent<CS_MatchStartPanel>();
        gameFinishPanelCS = gameFinishPanel.GetComponent<CS_GameFinishPanel>();
        matchPanelCS = matchPanel.GetComponent<CS_MatchPanel>();
    }

    // /// ///////////////////////////////////////////////////////////////////
    // /// Panel

    public CS_GamePanel GetGamePanel(){
        return gamePanelCS;
    }

    // public CS_TouchPanel GetTouchPanel(){
    //     return touchPanelCS;
    // }

    public CS_MatchStartPanel GetMatchStartPanel(){
        return matchStartPanelCS;
    }

    public CS_GameFinishPanel GetGameFinishPanel(){
        return gameFinishPanelCS;
    }

    public CS_MatchPanel GetMatchPanel(){
        return matchPanelCS;
    }
}

public interface PanelSetting
{
    void Init();
    void Open();
    void Close();
}
