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


        //Ett dictionary enemyValues inneh�ller alla fiendetyper och vad de kostar, vilket anv�nds n�r spelet "k�per" fiender att skapa
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

        // Varje v�g har en "budget"; spelet k�per fiender som ska spawnas tills budgeten �r slut
        while (budget > 0)
        {
            // V�ljer slumpm�ssigt en typ av fiende. Fienderna har olika frekvenser som avg�r hur stor chans de har att v�ljas
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

            // Fienderna som ska komma i varje wave l�ggs i en "pool" och skapas sedan i update.
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
            // V�ljer slumpm�ssigt en kant av sk�rmen, och sedan en slumpvald punkt strax utanf�r kanten d�r fienden ska spawna.
            if (randomNumber <= 0.2) spawnPosition = new Vector2(1.1f, Random.Range(-0.1f, 1.1f));
            else if (randomNumber <= 0.4) spawnPosition = new Vector2(-0.1f, Random.Range(-0.1f, 1.1f));
            else if (randomNumber <= 0.7) spawnPosition = new Vector2(Random.Range(-0.1f, 1.1f), 1.1f);
            else spawnPosition = new Vector2(Random.Range(-0.1f, 1.1f), -0.1f);

            //Den f�rsta fienden i enemyPool skapas och tas bort fr�n listan
            GameObject newEnemy = Instantiate(enemyPool[0], Camera.main.ViewportToWorldPoint(spawnPosition, 0), 
                Quaternion.identity, mastermind.enemiesContainer);
            newEnemy.GetComponent<Enemy>().player = mastermind.player.transform;
            newEnemy.GetComponent<Enemy>().mastermind = mastermind;
            newEnemy.GetComponent<Enemy>().value = enemyValues[enemyPool[0]];
            enemyPool.RemoveAt(0);


        }

    }





}
