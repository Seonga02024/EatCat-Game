using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CS_mainMenuPanel : MonoBehaviour, PanelSetting
{
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private Button soloModeBtn;
    [SerializeField] private Button MTModeBtn;
    [SerializeField] private Button drawStoreBtn;
    [SerializeField] private Button storeBtn;
    [SerializeField] private Button decorateBtn;
    [SerializeField] private Button settingBtn;
    [SerializeField] private Button infoBackBtn;
    [SerializeField] private GameObject info;

    void Start()
    {
        Init();
    }

    public void Init()
    {
        soloModeBtn.onClick.AddListener(OnSoloModeClicked);
        MTModeBtn.onClick.AddListener(OnMTModeClicked);
        drawStoreBtn.onClick.AddListener(OnDrawStoreClicked);
        storeBtn.onClick.AddListener(OnStoreClicked);
        decorateBtn.onClick.AddListener(OnDecorateClicked);
        settingBtn.onClick.AddListener(OnSettingClicked);
        infoBackBtn.onClick.AddListener(infoBackClicked);
    }

    public void Open()
    {
        mainPanel.SetActive(true);
    }

    public void Close()
    {
        mainPanel.SetActive(false);
    }

    private void OnSoloModeClicked()
    {
        Debug.Log("Solo Mode Button Clicked");
        CS_GameSoundManager.Instance.SfxPlay(SFX.Click_SFX);
        CS_GameSoundManager.Instance.BgmSet(BGM.SoloGame_BGM);
        CS_GameSoundManager.Instance.BgmPlay();
        CS_SoloGameManager.Instance.GameEnter();
    }

    private void OnMTModeClicked()
    {
        Debug.Log("Multiplayer Mode Button Clicked");
        // Multiplayer mode 시작 로직 추가\
        CS_MTGameManager.Instance.GameEnter();
        CS_GameSoundManager.Instance.SfxPlay(SFX.Click_SFX);
        CS_GameSoundManager.Instance.BgmSet(BGM.MTGame_BGM);
        CS_GameSoundManager.Instance.BgmPlay();
    }

    private void OnDrawStoreClicked()
    {
        Debug.Log("Draw Store Button Clicked");
        CS_GameSoundManager.Instance.SfxPlay(SFX.Click_SFX);
        // Draw store 로직 추가
    }

    private void OnStoreClicked()
    {
        Debug.Log("Store Button Clicked");
        CS_GameSoundManager.Instance.SfxPlay(SFX.Click_SFX);
        // Store 로직 추가
    }

    private void OnDecorateClicked()
    {
        Debug.Log("Decorate Button Clicked");
        CS_GameSoundManager.Instance.SfxPlay(SFX.Click_SFX);
        // Decorate 로직 추가
    }

    private void OnSettingClicked()
    {
        Debug.Log("Setting Button Clicked");
        CS_GameSoundManager.Instance.SfxPlay(SFX.Click_SFX);
        info.SetActive(true);
        // Setting 로직 추가
    }

    private void infoBackClicked()
    {
        info.SetActive(false);
        CS_GameSoundManager.Instance.SfxPlay(SFX.Click_SFX);
    }
}
