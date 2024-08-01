using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_CanvasController : SingleTon<CS_CanvasController>
{
    [SerializeField]
    public GameObject gamePanel;
    private CS_GamePanel gamePanelCS;

    [SerializeField]
    public GameObject touchPanel;
    private CS_TouchPanel touchPanelCS;

    void Start()
    {
        gamePanelCS = gamePanel.GetComponent<CS_GamePanel>();
        touchPanelCS = touchPanel.GetComponent<CS_TouchPanel>();
    }

    // /// ///////////////////////////////////////////////////////////////////
    // /// Panel

    public CS_GamePanel GetGamePanel(){
        return gamePanelCS;
    }

    public CS_TouchPanel GetTouchPanel(){
        return touchPanelCS;
    }
}

public interface PanelSetting
{
    void Init();
    void Open();
    void Close();
}
