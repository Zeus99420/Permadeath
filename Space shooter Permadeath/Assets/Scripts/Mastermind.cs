using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Mastermind : MonoBehaviour
{
    public GameObject player;
    public PermadeathScreen permadeathscreen;
    public WaveSpawner waveSpawner;

    public Text moneyText;
    public Text scoreText;

    [HideInInspector] public int money;
    [HideInInspector] public int score;

    public enum GameMastermindState
    {
        Opening,
        Gameplay,
        GameOver,
    }

    GameMastermindState GMState;
    void Start()
    {

      
        //GMState = GameMastermindState.Opening;
        SetGameMastermindState(GameMastermindState.Opening);




    }

    void UpdateGameMastermindState()
    {
        switch (GMState)
        {
            case GameMastermindState.Opening:
                StartCoroutine(permadeathscreen.FadeOut());
                UpdateMoneyAndScore(0);
                break;
            case GameMastermindState.Gameplay:
                waveSpawner.enabled = true;
                break;
            case GameMastermindState.GameOver:
                waveSpawner.enabled = false;
                StartCoroutine(permadeathscreen.FadeIn());
                Invoke("ChangeToOpeningState", 6f);
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


    
    public void UpdateMoneyAndScore(int enemyValue)
    {
        money += enemyValue;
        score += enemyValue;
        moneyText.text = "Money: " + money;
        scoreText.text = "Score: " + score;

    }


}
