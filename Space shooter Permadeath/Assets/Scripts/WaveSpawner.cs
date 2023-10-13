using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Wave
{
    public string waveName;
    public float budget;
    public Enemies[] enemies;
    public float spawnRate;
    public bool shopAfter;
}

[System.Serializable]
public class EnemyTypes
{
    public GameObject type;
    public int value;
    public int appears;
}

 [System.Serializable] public class Enemies
{
    public GameObject type;
    public float frequency;
}

public class WaveSpawner : MonoBehaviour
{
    public Text waveText;
    Mastermind mastermind;

    public EnemyTypes[] allEnemyTypes;
    public List<GameObject> pickupList;
    public GameObject freighterPrefab;
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

    public void OLDNewWave()
    {
        currentWave = waves[nextWaveNumber];
        nextWaveNumber++;
        waveText.text = currentWave.waveName;
        budget = currentWave.budget;

        enemyFrequencySum = 0;


        foreach (Enemies enemy in currentWave.enemies)
        {
            enemyFrequencySum += enemy.frequency;
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
                chanceToPick += currentWave.enemies[i].frequency;
                if (randomNumber <= chanceToPick)
                {
                    enemyType = currentWave.enemies[i].type;
                }
                i++;
            }

            // Fienderna som ska komma i varje wave l�ggs i en "pool" och skapas sedan i update.
            enemyPool.Add(enemyType);
            budget -= enemyValues[enemyType];

        }

        //L�gger slumpm�ssigt till fraktskepp med pickups till enemypool. H�gre budget -> Fler fraktskepp, men inte linj�rt.
        for (int t = 0; t < Mathf.Sqrt(currentWave.budget); t++)
        {
            int randomIndex = Random.Range(0, enemyPool.Count+1);
            if (Random.value < (1f / 15f))
            {
                enemyPool.Insert(randomIndex, freighterPrefab);
            }
        }
        mastermind.CountEnemies();
    }


    public int baseBudget;
    //public int budgetIncrease;
    public float budgetExponent;
    public float baseSpawnRate;
    public float spawnRateIncrease;
    public void NewWave()
    {
        nextWaveNumber++;
        int waveBudget = (int)(baseBudget * nextWaveNumber * Mathf.Pow(budgetExponent, nextWaveNumber));
        //int waveBudget = baseBudget;
        //for (int i = 0; i < nextWaveNumber; i++) waveBudget += (int)(budgetIncrease * Mathf.Pow(budgetExponent, i));

        //spawnRate = baseSpawnRate * Mathf.Pow(spawnRateExponent, nextWaveNumber);
        spawnRate = baseSpawnRate + spawnRateIncrease * nextWaveNumber;
        budget = waveBudget;
        waveText.text = "Wave " + nextWaveNumber;
        Debug.Log("WAVE " + nextWaveNumber + ", Budget: " + budget + ", Spawn Rate: " + spawnRate);



        // Varje v�g har en "budget"; spelet k�per fiender som ska spawnas tills budgeten �r slut
        while (budget > 0)
        {
            int randomIndex = Random.Range(0, allEnemyTypes.Length);   // V�lj slumpm�ssigt en fiendetyp
            EnemyTypes batchType = allEnemyTypes[randomIndex];

            if (batchType.appears <= nextWaveNumber)    // Kollar om typen finns i denna wave
            {
                float batchBudget = 30 + Random.Range(0, waveBudget / 2);     //K�p fiender av typen f�r en budget av slumpad storlek
                if (batchBudget > budget) batchBudget = budget;

                while (batchBudget > 0)
                {
                    //enemyPool.Add(batchType.type);
                    randomIndex = Random.Range(0, enemyPool.Count);
                    enemyPool.Insert(randomIndex, batchType.type);
                    budget -= batchType.value;
                    batchBudget -= batchType.value;
                }
            }
        }

        //L�gger slumpm�ssigt till fraktskepp med pickups till enemypool. H�gre budget -> Fler fraktskepp, men inte linj�rt.
        for (int t = 0; t < Mathf.Sqrt(waveBudget); t++)
        {
            if (Random.value < (1f / 20f))
            {
                int randomIndex = Random.Range(0, enemyPool.Count);
                enemyPool.Insert(randomIndex, freighterPrefab);
            }
        }

        mastermind.CountEnemies();
    }

    public float spawnRate;
    //public float spawnRateExponent;
    private void Update()
    {
        Vector2 spawnPosition;
        if (enemyPool.Count > 0 && (mastermind.enemiesContainer.childCount == 0 || Random.value*enemyValues[enemyPool[0]] < Time.deltaTime * spawnRate))
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
