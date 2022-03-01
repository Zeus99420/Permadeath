using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Mastermind : MonoBehaviour
{
    public GameObject player;
    public GameObject permadeathscreen;
    public GameObject SpawnEnemies;
   // public Transform HealthBar;
    public enum GameMastermindState
    {
        Opening,
        Gameplay,
        GameOver,
    }

    GameMastermindState GMState;
    void Start()
    {

      // Transform healthBarTransform = Instantiate(HealthBar, new Vector3(0, 10), Quaternion.identity);
      //  HealthBar healthbar = healthBarTransform.GetComponent<HealthBar>();
      //  Health health = new Health(100);
        GMState = GameMastermindState.Opening;
     //   healthbar.Setup(health);
       
    
    }

    void UpdateGameMastermindState()
    {
        switch (GMState)
        {
            case GameMastermindState.Opening:
                permadeathscreen.SetActive(false);
                break;
            case GameMastermindState.Gameplay:
                SpawnEnemies.SetActive(true);
                break;
            case GameMastermindState.GameOver:
                SpawnEnemies.SetActive(false);
                permadeathscreen.SetActive(true);
                Invoke("ChangeToOpeningState", 12f);
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
