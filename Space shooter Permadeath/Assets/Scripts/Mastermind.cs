using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Mastermind : MonoBehaviour
{
    public GameObject player;
    public PermadeathScreen permadeathscreen;
    public WaveSpawner waveSpawner;
    public Shop shop;
    public Transform enemiesContainer;
    public Text enemiesRemainingText;


    public Text moneyText;
    public Text scoreText;

    [HideInInspector] public int money;
    [HideInInspector] public int score;

    public enum GameMastermindState
    {
        Opening,
        Gameplay,
        Shop,
        GameOver,
    }

    GameMastermindState GMState;
    void Start()
    {

        shop.Initialize();

      
        //GMState = GameMastermindState.Opening;
        SetGameMastermindState(GameMastermindState.Opening);




    }

    void UpdateGameMastermindState()
    {
        switch (GMState)
        {
            case GameMastermindState.Opening:
                StartCoroutine(permadeathscreen.FadeOut());
                UpdateMoney(0);
                UpdateScore(0);
                break;
            case GameMastermindState.Gameplay:
                waveSpawner.enabled = true;
                break;
            case GameMastermindState.Shop:
                shop.gameObject.SetActive(true);
                shop.EnterShop();
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

    public void EnterShop()
    {
        GMState = GameMastermindState.Shop;
        UpdateGameMastermindState();
    }

    public void ExitShop()
    {
        shop.Exit();

        GMState = GameMastermindState.Gameplay;
        UpdateGameMastermindState();

        waveSpawner.NewWave();
    }

    public void ChangeToOpeningState()
    {
        SetGameMastermindState(GameMastermindState.Opening);
    }

    public void CountEnemies()
    {
        int enemiesRemaining = waveSpawner.enemyPool.Count + enemiesContainer.childCount;
        enemiesRemainingText.text = ("Enemies Remaining: " + enemiesRemaining);

        //Startar en ny våg ifall alla fiender är förstörda
        if (enemiesRemaining == 0)
        {
            if (waveSpawner.currentWave.shopAfter) EnterShop();

            else waveSpawner.NewWave();
        }
    }

    public void UpdateMoney(int changeAmount)
    {
        money += changeAmount;
        moneyText.text = "Money: " + money;


    }

    public void UpdateScore(int changeAmount)
    {
        score += changeAmount;
        scoreText.text = "Score: " + score;

    }


}
