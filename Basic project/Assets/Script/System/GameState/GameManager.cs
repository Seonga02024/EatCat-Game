using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingleTon<GameManager>
{
    private IGameState _currentState;

    public LobbyState LobbyState;
    public SoloGameState SoloGameState;
    public MultiplayerGameState MultiplayerGameState;

    void Start()
    {
        LobbyState = new LobbyState(this);
        SoloGameState = new SoloGameState(this);
        MultiplayerGameState = new MultiplayerGameState(this);

        SetState(LobbyState);
    }

    void Update()
    {
        if (_currentState != null)
        {
            _currentState.UpdateState();
        }
    }

    public void SetState(IGameState newState)
    {
        if (_currentState != null)
        {
            _currentState.ExitState();
        }

        _currentState = newState;
        _currentState.EnterState();
    }
}
