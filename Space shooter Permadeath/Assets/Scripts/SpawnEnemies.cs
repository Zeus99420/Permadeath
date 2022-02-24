using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    public GameObject enemy;
    public float spawnRate;
    public float spawnRateIncrease;
    public Vector2 spawnPosition;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Testar om en fiende ska spawnas
        float randomNumber = Random.value;
        if (randomNumber < Time.deltaTime*spawnRate )
        {
            randomNumber = Random.value;
            // V�ljer slumpm�ssigt en kant av sk�rmen, och sedan en slumpvald punkt strax utanf�r kanten d�r fienden ska spawna.
            if (randomNumber <= 0.2) spawnPosition = new Vector2(1.1f, Random.Range(-0.1f, 1.1f));
            else if (randomNumber < 0.4) spawnPosition = new Vector2(-0.1f, Random.Range(-0.1f, 1.1f));
            else if (randomNumber < 0.7) spawnPosition = new Vector2(Random.Range(-0.1f, 1.1f), 1.1f);
            else spawnPosition = new Vector2(Random.Range(-0.1f, 1.1f), -0.1f);

            GameObject newEnemy = Instantiate(enemy, Camera.main.ViewportToWorldPoint(spawnPosition,0), Quaternion.identity);
            newEnemy.GetComponent<Enemy>().player = GetComponent<Mastermind>().player.transform;
        }
        spawnRate += spawnRate*spawnRateIncrease*Time.deltaTime;
    }
}
