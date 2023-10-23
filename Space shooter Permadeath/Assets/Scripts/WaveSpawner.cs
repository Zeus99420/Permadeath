using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable] public class EnemyTypes
{
    public GameObject type;
    public int value;
    public int appears;
}

public class WaveSpawner : MonoBehaviour
{
    public Text waveText;
    Mastermind mastermind;

    public EnemyTypes[] allEnemyTypes;
    public List<GameObject> pickupList;
    public GameObject freighterPrefab;
    public Dictionary<GameObject, int> enemyValues = new Dictionary<GameObject, int>();

    [HideInInspector] public int nextWaveNumber = 0;
    [HideInInspector] public int budget = 0;

    public List<GameObject> enemyPool;

    public int baseBudget;
    //public int budgetIncrease;
    public float budgetExponent;
    float spawnRate;
    public float baseSpawnRate;
    public float spawnRateIncrease;

    public List<int> bossLevels;
    public GameObject boss;
    public CoolHealthBar bossHealthbar;
    GameObject nextEnemy;
    [HideInInspector] public bool bossFight;


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
        //F�rbereder vilka fiender spelaren ska m�te i en v�g.
        nextWaveNumber++;

        if (bossLevels.Contains(nextWaveNumber))
        {
            InitiateBossFight();
            spawnRate = (baseSpawnRate + spawnRateIncrease * nextWaveNumber)*0.1f;

            waveText.text = "BOSSFIGHT";
        }

        else
        {
            int waveBudget = (int)(baseBudget * nextWaveNumber * Mathf.Pow(budgetExponent, nextWaveNumber));
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
                    int batchBudget = 30 + Random.Range(0, waveBudget / 2);     //K�p fiender av typen f�r en budget av slumpad storlek
                    if (batchBudget > budget) batchBudget = budget;

                    while (batchBudget > 0)
                    {
                        randomIndex = Random.Range(0, enemyPool.Count + 1);
                        enemyPool.Insert(randomIndex, batchType.type);
                        budget -= batchType.value;
                        batchBudget -= batchType.value;
                    }
                }
            }

            //L�gger slumpm�ssigt till fraktskepp med pickups till enemypool. H�gre budget -> Fler fraktskepp, men inte linj�rt.
            for (int t = 0; t < Mathf.Sqrt(waveBudget / 10); t++)
            {
                if (Random.value < (1f / 4f))
                {
                    int randomIndex = Random.Range(0, enemyPool.Count + 1);
                    enemyPool.Insert(randomIndex, freighterPrefab);
                }
            }
        }

        mastermind.CountEnemies();
    }






    private void Update()
    {
        if (bossFight) BossFightUpdate();
        else StandardUpdate();

    }



    void StandardUpdate()
    {
        if (enemyPool.Count > 0 && (mastermind.enemiesContainer.childCount == 0 || Random.value * enemyValues[enemyPool[0]] < Time.deltaTime * spawnRate))
        {
            SpawnEnemy(enemyPool[0], 0.1f);
            enemyPool.RemoveAt(0);
        }
    }

    Vector2 GetSpawnPosition(float margin)
    {
        Vector2 spawnPosition;
        float randomNumber = Random.value;
        // V�ljer slumpm�ssigt en kant av sk�rmen, och sedan en slumpvald punkt strax utanf�r kanten d�r fienden ska spawna.
        if (randomNumber <= 0.2) spawnPosition = new Vector2(1+margin, Random.Range(0-margin, 1 + margin));
        else if (randomNumber <= 0.4) spawnPosition = new Vector2(0 - margin, Random.Range(0 - margin, 1 + margin));
        else if (randomNumber <= 0.7) spawnPosition = new Vector2(Random.Range(0 - margin, 1 + margin), 1 + margin);
        else spawnPosition = new Vector2(Random.Range(0 - margin, 1 + margin), 0 - margin);

        return spawnPosition;
    }

    GameObject SpawnEnemy(GameObject enemy, float spawnMargin)
    {
        GameObject newEnemy = Instantiate(enemy, Camera.main.ViewportToWorldPoint(GetSpawnPosition(spawnMargin), 0),
            Quaternion.identity, mastermind.enemiesContainer);
        newEnemy.GetComponent<Enemy>().player = mastermind.player.transform;
        newEnemy.GetComponent<Enemy>().mastermind = mastermind;
        newEnemy.GetComponent<Enemy>().value = enemyValues[enemy];
        return newEnemy;
    }




    void InitiateBossFight()
    {
        bossFight = true;
        GameObject newBoss = SpawnEnemy(boss, 0.2f);
        newBoss.GetComponent<BossEnemy>().healthBar = bossHealthbar;
        /*nextEnemy = */GetNextEnemy();
    }

    void BossFightUpdate()
    {
        if (Random.value * enemyValues[nextEnemy] < Time.deltaTime * spawnRate)
        {
            SpawnEnemy(nextEnemy, 0.1f);
            mastermind.CountEnemies();
            /*nextEnemy =*/ GetNextEnemy();
        }
    }

    void GetNextEnemy()
    {
        bool searching = true;
        while (searching)
        {
            int randomIndex = Random.Range(0, allEnemyTypes.Length);   // V�lj slumpm�ssigt en fiendetyp
            EnemyTypes enemyType = allEnemyTypes[randomIndex];

            if (enemyType.appears <= nextWaveNumber && Random.value * enemyValues[enemyType.type] < 10)    // Kollar om typen finns i denna wave
            {
                searching = false;
                nextEnemy = enemyType.type;
            }
        }
    }





}
