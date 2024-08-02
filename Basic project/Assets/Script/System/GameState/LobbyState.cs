using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyState : IGameState
{
    private GameManager _gameManager;

    public LobbyState(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    public void EnterState()
    {
        Debug.Log("Entered Lobby State");
        // 로비 화면을 활성화하고 필요한 초기화를 수행합니다.
    }

    public void UpdateState()
    {
        // 로비 상태에서 필요한 업데이트 로직을 수행합니다.
    }

    public void ExitState()
    {
        Debug.Log("Exiting Lobby State");
        // 로비 상태를 벗어날 때 필요한 정리 작업을 수행합니다.
    }
}
