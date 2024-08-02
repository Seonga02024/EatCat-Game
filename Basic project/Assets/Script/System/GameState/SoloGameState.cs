using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoloGameState : IGameState
{
    private GameManager _gameManager;

    public SoloGameState(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    public void EnterState()
    {
        Debug.Log("Entered Solo Game State");
        // 솔로 게임 모드 초기화 로직을 수행합니다.
    }

    public void UpdateState()
    {
        // 솔로 게임 모드에서 필요한 업데이트 로직을 수행합니다.
    }

    public void ExitState()
    {
        Debug.Log("Exiting Solo Game State");
        // 솔로 게임 모드를 벗어날 때 필요한 정리 작업을 수행합니다.
    }
}
