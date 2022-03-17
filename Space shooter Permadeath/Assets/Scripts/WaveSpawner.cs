using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Wave
{
    public string waveName;
    public float budget;
    public GameObject[] enemyTypes;
    public float[] enemyFrequencies;
    public float spawnRate;
    public bool shopAfter;
}

[System.Serializable]
public class EnemyTypes
{
    public GameObject type;
    public int value;
}

public class WaveSpawner : MonoBehaviour
{
    public Text waveText;
    Mastermind mastermind;

    public EnemyTypes[] allEnemyTypes;
    public Dictionary<GameObject, int> enemyValues = new Dictionary<GameObject, int>();

    public Wave[] waves;
    [HideInInspector] public int nextWaveNumber = 0;
    [HideInInspector] public float budget = 0;

    [HideInInspector] public Wave currentWave;
    public List<GameObject> enemyPool;
    float enemyFrequencySum;


    public void Start()
    {
        mastermind = GameObject.Find("Mastermind").GetComponent<Mastermind>();


        //Ett dictionary enemyValues innehåller alla fiendetyper och vad de kostar, vilket används när spelet "köper" fiender att skapa
        foreach (EnemyTypes enemyType in allEnemyTypes)
        {
            enemyValues.Add(enemyType.type, enemyType.value);
        }

    }

    public void NewWave()
    {
        currentWave = waves[nextWaveNumber];
        nextWaveNumber++;
        waveText.text = currentWave.waveName;
        budget = currentWave.budget;

        enemyFrequencySum = 0;
        foreach (float frequency in currentWave.enemyFrequencies)
        {
            enemyFrequencySum += frequency;
        }

        // Varje våg har en "budget"; spelet köper fiender som ska spawnas tills budgeten är slut
        while (budget > 0)
        {
            // Väljer slumpmässigt en typ av fiende. Fienderna har olika frekvenser som avgör hur stor chans de har att väljas
            GameObject enemyType = null;
            float randomNumber = Random.Range(0, enemyFrequencySum);
            int i = 0;
            float chanceToPick = 0;
            while (enemyType == null)
            {
                chanceToPick += currentWave.enemyFrequencies[i];
                if (randomNumber <= chanceToPick)
                {
                    enemyType = currentWave.enemyTypes[i];
                }
                i++;
            }

            // Fienderna som ska komma i varje wave läggs i en "pool" och skapas sedan i update.
            enemyPool.Add(enemyType);
            budget -= enemyValues[enemyType];

        }
        mastermind.CountEnemies();
    }

    private void Update()
    {
        Vector2 spawnPosition;
        if (enemyPool.Count>0 && Random.value < Time.deltaTime * currentWave.spawnRate)
        {
            float randomNumber = Random.value;
            // Väljer slumpmässigt en kant av skärmen, och sedan en slumpvald punkt strax utanför kanten där fienden ska spawna.
            if (randomNumber <= 0.2) spawnPosition = new Vector2(1.1f, Random.Range(-0.1f, 1.1f));
            else if (randomNumber <= 0.4) spawnPosition = new Vector2(-0.1f, Random.Range(-0.1f, 1.1f));
            else if (randomNumber <= 0.7) spawnPosition = new Vector2(Random.Range(-0.1f, 1.1f), 1.1f);
            else spawnPosition = new Vector2(Random.Range(-0.1f, 1.1f), -0.1f);

            //Den första fienden i enemyPool skapas och tas bort från listan
            GameObject newEnemy = Instantiate(enemyPool[0], Camera.main.ViewportToWorldPoint(spawnPosition, 0), 
                Quaternion.identity, mastermind.enemiesContainer);
            newEnemy.GetComponent<Enemy>().player = mastermind.player.transform;
            newEnemy.GetComponent<Enemy>().mastermind = mastermind;
            newEnemy.GetComponent<Enemy>().value = enemyValues[enemyPool[0]];
            enemyPool.RemoveAt(0);


        }

    }





}
