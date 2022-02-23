using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mastermind : MonoBehaviour
{
    public GameObject Player;

    public enum GameMastermindState
    {
        Opening,
        Gameplay,
        GameOver,
    }

    GameMastermindState GMState;
    void Start()
    {
        GMState = GameMastermindState.Opening;
    }

    void UpdateGameMastermindState()
    {
        switch (GMState)
        {
            case GameMastermindState.Opening:
                break;
            case GameMastermindState.Gameplay:
                break;
            case GameMastermindState.GameOver:
                break;
        }
    }

    public void SetGameMastermindState(GameMastermindState state)
    {
        GMState = state;
        UpdateGameMastermindState();
    }

    public void StartGameplay()
    {
        GMState = GameMastermindState.Gameplay;
        UpdateGameMastermindState();
    }

    public void ChangeToOpeningState()
    {
        SetGameMastermindState(GameMastermindState.Opening);
    }


    
    void Update()
    {
        
    }
}
