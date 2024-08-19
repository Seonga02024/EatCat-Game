using UnityEngine;
using System.IO;

[System.Serializable]
public class GameData
{
    public int high_score;
    public int solo_coin;
    public int mt_coin;

    public GameData()
    {
        high_score = 0;
        solo_coin = 0;
        mt_coin = 0;
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
        GetHighScore();
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

    public void PlusSoloCoin(int coin) { GameData.solo_coin += coin; }
    public void MinusSoloCoin(int coin) { GameData.solo_coin -= coin; } 
    public int GetSoloCoin() { return GameData.solo_coin; } 
    public void PlusMTCoin(int coin) { GameData.mt_coin += coin; } 
    public void MinusMTCoin(int coin) { GameData.mt_coin -= coin; } 
    public int GetMTCoin() { return GameData.mt_coin; } 
    public void SetHighScore(int high_score) { GameData.high_score = GameData.high_score > high_score ? GameData.high_score : high_score; } 
    public int GetHighScore() { return GameData.high_score; } 

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
