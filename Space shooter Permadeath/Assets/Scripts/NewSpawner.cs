using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class EnemyType
{
    public GameObject type;
    public int value;
    public int appears;
}

public class NewSpawner : MonoBehaviour
{
    public EnemyType[] enemyTypes;

    public Text waveText;
    Mastermind mastermind;

    public List<GameObject> pickupList;
    public GameObject freighterPrefab;
    public Dictionary<GameObject, int> enemyValues = new Dictionary<GameObject, int>();

    [HideInInspector] public int nextWaveNumber = 0;
    [HideInInspector] public int budget = 0;

    public List<GameObject> enemyPool;

    public void Start()
    {
        mastermind = GameObject.Find("Mastermind").GetComponent<Mastermind>();


        //Ett dictionary enemyValues innehåller alla fiendetyper och vad de kostar, vilket används när spelet "köper" fiender att skapa
        foreach (EnemyType enemyType in enemyTypes)
        {
            enemyValues.Add(enemyType.type, enemyType.value);
        }

    }

    public int baseBudget;
    public float budgetExponent;

    public void NewWave()
    {
        int waveBudget = (int)(baseBudget *Mathf.Pow(budgetExponent,nextWaveNumber));
        budget = waveBudget;
        nextWaveNumber++;
        Debug.Log("budget: " + budget);

        while (budget > 0)
        {
            int randomIndex = Random.Range(0, enemyTypes.Length);   // Välj slumpmässigt en fiendetyp
            EnemyType batchType = enemyTypes[randomIndex];
            
            if (batchType.appears <= nextWaveNumber)    // Kollar om typen finns i denna wave
            {
                int batchBudget = 30 + Random.Range(0, waveBudget / 2);     //Köp fiender av typen för en budget av slumpad storlek
                if (batchBudget > budget) batchBudget = budget;

                while (batchBudget>0)
                {
                    enemyPool.Add(batchType.type);
                    budget -= batchType.value;
                    batchBudget -= batchType.value;
                }
            }
        }

        //Lägger slumpmässigt till fraktskepp med pickups till enemypool. Högre budget -> Fler fraktskepp, men inte linjärt.
        //for (int t = 0; t < Mathf.Sqrt(waveBudget); t++)
        //{
        //    int randomIndex = Random.Range(0, enemyPool.Count + 1);
        //    if (Random.value < (1f / 15f))
        //    {
        //        enemyPool.Insert(randomIndex, freighterPrefab);
        //    }
        //}
        mastermind.CountEnemies();
    }

    public float spawnBudget;
    public float spawnPerSecond;
    void Update()
    {
        spawnBudget += spawnPerSecond * Time.deltaTime;
        //if (enemyPool.Count > 0)
        //{
        //    spawnBudget += spawnPerSecond * Time.deltaTime;
        //    //if (spawnBudget > enemyValues[enemyPool[0]])
        //    //{
        //    //    SpawnEnemy();
        //    //    spawnBudget -= enemyValues[enemyPool[0]];
        //    //}
        //}
    }

    void SpawnEnemy()
    {
        Vector2 spawnPosition;
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
