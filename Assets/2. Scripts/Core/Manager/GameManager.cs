using System;
using Scripts.Manager;
using Scripts.Utils;

public class GameManager : Singleton<GameManager>
{
    public GameState CurrentGameState { get; private set; }
    
    public event Action<GameState> OnGameStateChanged;

    public void ChangeGameState(GameState newState)
    {
        CurrentGameState = newState;
        OnGameStateChanged?.Invoke(newState);   
    }
}