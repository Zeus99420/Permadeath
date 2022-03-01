using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave
{
    public string waveName;
    public float budget;
    public GameObject[] enemyTypes;
    public float[] enemyFrequencies;
    public float spawnRate;
}

[System.Serializable]
public class EnemyTypes
{
    public GameObject type;
    public int value;
}

public class WaveSpawner : MonoBehaviour
{
    Mastermind mastermind;
    public Transform enemiesContainer;

    public EnemyTypes[] allEnemyTypes;
    Dictionary<GameObject, int> enemyValues = new Dictionary<GameObject, int>();


    public Wave[] waves;
    public int currentWaveNumber = 0;
    Wave currentWave;
    public List<GameObject> enemyPool;
    float enemyFrequencySum;

    public int enemiesRemaining;

    public void Start()
    {
        mastermind = GameObject.Find("Mastermind").GetComponent<Mastermind>();
        enemiesContainer = GameObject.Find("Enemies").GetComponent<Transform>();


        //Ett dictionary enemyValues inneh�ller alla fiendetyper och vad de kostar, vilket anv�nds n�r spelet "k�per" fiender att skapa
        foreach (EnemyTypes enemyType in allEnemyTypes)
        {
            enemyValues.Add(enemyType.type, enemyType.value);
        }

        InitializeWave();
    }

    public void InitializeWave()
    {
        currentWave = waves[currentWaveNumber];

        enemyFrequencySum = 0;
        foreach (float frequency in currentWave.enemyFrequencies)
        {
            enemyFrequencySum += frequency;
        }

        // Varje v�g har en "budget"; spelet k�per fiender som ska spawnas tills budgeten �r slut
        while (currentWave.budget > 0)
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
            currentWave.budget -= enemyValues[enemyType];
        }
        CountEnemies();
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
                Quaternion.identity, enemiesContainer);
            newEnemy.GetComponent<Enemy>().player = mastermind.player.transform;
            enemyPool.RemoveAt(0);


        }

        CountEnemies();
    }

    public void CountEnemies()
    {
        enemiesRemaining = enemyPool.Count + enemiesContainer.childCount;

        //Startar en ny v�g ifall alla fiender �r f�rst�rda
        if (enemiesRemaining == 0)
        {
            currentWaveNumber++;
            InitializeWave();
        }
    }

}
