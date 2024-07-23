using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

[System.Serializable]
public class StoryInfo
{
    public List<bool> isStoryOpened;

    public StoryInfo()
    {
        isStoryOpened = new List<bool>();
        for (int i = 0; i < 3; i++)
        {
            isStoryOpened.Add(false); //초기값 false
        }
    }

    public void openStory(int index) { isStoryOpened[index] = true; } //스토리 해금
}

[System.Serializable]
public class GameData
{
    public int high_score;
    public int coin;
    public List<int> upgrade;
    public int heart;
    public bool isGameFirst;
    public List<StoryInfo> unlockedMemory;
    public float bgmSound;
    public float sfxSound;
    public int playCount;
    public bool haptic;

    public GameData()
    {
        high_score = 0;
        coin = 0;

        upgrade = new List<int>();
        for (int i = 0; i < 3; i++) upgrade.Add(0);

        heart = 5;
        isGameFirst = false;

        //10개의 스테이지
        unlockedMemory = new List<StoryInfo>();
        for (int i = 0; i < 10; i++) unlockedMemory.Add(new StoryInfo());

        bgmSound = 0;
        sfxSound = 0;
        playCount = 0;
        haptic = true;
    }
}

public class CS_SaveLoadManager : SingleTon<CS_SaveLoadManager>
{
    private string savePath;
    private GameData _gameData;

    public GameData GameData
    {
        get
        {
            if(_gameData == null)
            {
                _gameData = LoadData();
                SaveData();
            }

            return _gameData;
        }
    }

    private void Start()
    {
        // Application.persistentDataPath는 각 플랫폼에 따라 저장될 수 있는 영구적인 데이터 경로를 제공합니다.
        savePath = Path.Combine(Application.persistentDataPath, "GameData.json");
        Debug.Log(Application.persistentDataPath);
    }

    private GameData LoadData()
    {
        Debug.Log(savePath);
        if (File.Exists(savePath))
        {
            // 파일에서 JSON 데이터 읽기
            string jsonData = File.ReadAllText(savePath);

            // JSON 데이터를 클래스로 변환
            GameData loadedData = JsonUtility.FromJson<GameData>(jsonData);
            return loadedData;
        }
        else
        {
            Debug.Log("새로운 파일 생성");
            GameData gameData = new GameData();

            return gameData;
        }
    }

    public void UpgradeHP() { GameData.upgrade[0]++; } //호출 시 upgrade_hp 1 증가
    public void UpgradeEnergy() { GameData.upgrade[1]++; } //호출 시 upgrade_energy 1 증가
    public void UpgradeJelly() { GameData.upgrade[2]++; } //호출 시 upgrade_jelly 1 증가
    public int GetUpgradeHP() { return GameData.upgrade[0]; } //upgrade_hp 반환
    public int GetUpgradeEnergy() { return GameData.upgrade[1]; } //upgrade_energy 반환
    public int GetUpgradeJelly() { return GameData.upgrade[2]; } //upgrade_jelly 반환
    public void PlusCoin(int coin) { GameData.coin += coin; } //Coin 더하기
    public void MinusCoin(int coin) { GameData.coin -= coin; } //Coin 빼기
    public int GetCoin() { return GameData.coin; } //DB에 있는 Coin 반환
    public void SetHighScore(int high_score) { GameData.high_score = GameData.high_score > high_score ? GameData.high_score : high_score; } //최대 점수 설정
    public int GetHighScore() { return GameData.high_score; } //최대 점수 반환
    public int GetHeart() { return GameData.heart; } //하트 반환
    public void PlusHeart() { GameData.heart++; } //하트 하나 더하기
    public void SubtractHeart() { GameData.heart--; } //하트 하나 빼기
    public bool GetIsGameFirst() { return GameData.isGameFirst; } //처음 시작 여부 반환
    public void SetIsGameFirst() { GameData.isGameFirst = true; } //처음 시작 완료 설정
    public List<StoryInfo> GetUnlockedMemory() { return GameData.unlockedMemory; } //먹은 기억의 조각 반환
    public void SetBgmSound(float sound) { GameData.bgmSound = sound; } //bgm 소리 저장
    public float GetBgmSound() { return GameData.bgmSound; } //bgm 소리 반환
    public void SetSfxSound(float sound) { GameData.sfxSound = sound; } //sfx 소리 저장
    public float GetSfxSound() { return GameData.sfxSound; } //sfx 소리 반환
    public void ReadStory(int concept, int index) { GameData.unlockedMemory[concept].openStory(index); } //스토리를 읽은 상태로 설정
    public void PlusPlayCount() { GameData.playCount++; } //플레이 횟수 + 1
    public int GetPlayCount() { return GameData.playCount; } //플레이 횟수 반환
    public bool ToggleHaptic() //Toggle haptic
    {
        GameData.haptic = !GameData.haptic;
        return GameData.haptic;
    }
    public bool GetHaptic() { return GameData.haptic; }

    public void SaveData()
    {
        // 데이터를 JSON 형식으로 변환
        string jsonData = JsonUtility.ToJson(_gameData);
        
        // JSON 데이터를 파일에 쓰기
        File.WriteAllText(savePath, jsonData);
        Debug.Log("저장 완료");
    }

    void OnApplicationQuit()
    {
        SaveData();
    }
}
