using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Mastermind : MonoBehaviour
{
    public bool testingShop;
    public bool permadeath;
    public GameObject player;
    UIScreen deathScreen;
    public UIScreen permadeathScreen;
    public UIScreen standardDeathScreen;
    public UIScreen instructions;
    public Sprite deathscreen;
    public Button startoverbutton;
    Coroutine deathScreenCoroutine;
    public WaveSpawner waveSpawner;
    public Shop shop;
    public Transform enemiesContainer;
    public Transform stuffContainer;
    public Text enemiesRemainingText;


    public Text moneyText;
    public Text scoreText;

    [HideInInspector] public int money;
    [HideInInspector] public int score;

    GameObject savedPlayer;
    int savedMoney;
    int savedScore;
    int savedNextWave;

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
        SetGameMastermindState(GameMastermindState.Opening);

        shop.Initialize();
        CheckpointSave();

        if (testingShop) { UpdateMoney(1000); EnterShop(); }
        else StartCoroutine(instructions.FadeIn());

        if (permadeath) deathScreen = permadeathScreen;
        else deathScreen = standardDeathScreen;
         








    }

    private void Update()
    {
        if (GMState == GameMastermindState.Opening && Input.GetKeyDown("space")) StartGameplay();

        if (GMState == GameMastermindState.Shop && Input.GetKeyDown("space")) ExitShop();

        if (GMState == GameMastermindState.GameOver && Input.GetKeyDown("t") && !permadeath) StartFromCheckpoint();

        if(/*GMState == GameMastermindState.GameOver &&*/ Input.GetKeyDown("r")) StartOver();

    }

    void UpdateGameMastermindState()
    {
        switch (GMState)
        {
            case GameMastermindState.Opening:
                UpdateMoney(0);
                UpdateScore(0);
                //startoverbutton.gameObject.SetActive(false);
                break;
            case GameMastermindState.Gameplay:
                StartCoroutine(instructions.FadeOut());
                waveSpawner.enabled = true;
                waveSpawner.Invoke("NewWave", 0f);
                break;
            case GameMastermindState.Shop:
                shop.gameObject.SetActive(true);
                shop.EnterShop();
                break;
            case GameMastermindState.GameOver:
                waveSpawner.enabled = false;
                deathScreenCoroutine = StartCoroutine(deathScreen.FadeIn());

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
        CheckpointSave();


        GMState = GameMastermindState.Gameplay;
        UpdateGameMastermindState();

        //waveSpawner.NewWave();
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
        if (GMState == GameMastermindState.Gameplay && enemiesRemaining == 0)
        {
            if (waveSpawner.currentWave.shopAfter)
            {
                EnterShop();
            }

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

    public void CheckpointSave()
    {
        if(savedPlayer) { Destroy(savedPlayer); }
        savedPlayer = Instantiate(player);
        savedPlayer.SetActive(false);
        savedMoney = money;
        savedScore = score;
        savedNextWave = waveSpawner.nextWaveNumber;
    }

    public void StartFromCheckpoint()
    {
        GMState = GameMastermindState.Gameplay;
        foreach (Transform enemy in enemiesContainer) Destroy(enemy.gameObject);
        foreach (Transform projectile in stuffContainer)  Destroy(projectile.gameObject);

        player = Instantiate(savedPlayer);
        player.SetActive(true);
        money = savedMoney;
        score = savedScore;
        UpdateMoney(0);
        UpdateScore(0);
        waveSpawner.enabled = true;
        waveSpawner.enemyPool.Clear();
        waveSpawner.nextWaveNumber = savedNextWave;
        waveSpawner.NewWave();
        StopCoroutine(deathScreenCoroutine);
        StartCoroutine(deathScreen.FadeOut());
        Invoke("CountEnemies", 0f);
    }

    public void StartOver ()
    {
        
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            
    }
}
