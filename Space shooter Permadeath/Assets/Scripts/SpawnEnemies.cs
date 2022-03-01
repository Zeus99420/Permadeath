using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    public GameObject enemyType;
    public List<GameObject> enemyTypes;
    public List<float> enemyFrequencies;

    public float enemyFrequencySum = 0;

    public float spawnRate;
    public float spawnRateIncrease;
    public Vector2 spawnPosition;
    // Start is called before the first frame update
    void Start()
    {
        foreach (float   frequency in enemyFrequencies)
        {
            enemyFrequencySum += frequency;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Testar om en fiende ska spawnas
        float randomNumber = Random.value;
        if (randomNumber < Time.deltaTime*spawnRate )
        {
            randomNumber = Random.value;
            // Väljer slumpmässigt en kant av skärmen, och sedan en slumpvald punkt strax utanför kanten där fienden ska spawna.
            if (randomNumber <= 0.2) spawnPosition = new Vector2(1.1f, Random.Range(-0.1f, 1.1f));
            else if (randomNumber < 0.4) spawnPosition = new Vector2(-0.1f, Random.Range(-0.1f, 1.1f));
            else if (randomNumber < 0.7) spawnPosition = new Vector2(Random.Range(-0.1f, 1.1f), 1.1f);
            else spawnPosition = new Vector2(Random.Range(-0.1f, 1.1f), -0.1f);


            // Väljer slumpmässigt en typ av fiende. Fienden har olika frekvenser som avgör hur stor chans de har att väljas
            enemyType = null;
            randomNumber = Random.Range(0, enemyFrequencySum);
            int i = 0;
            float chanceToPick = 0;
            while (enemyType == null)
            {
                chanceToPick += enemyFrequencies[i];
                if (randomNumber <= chanceToPick)
                {
                    enemyType = enemyTypes[i];
                }
                i++;
            }

            GameObject newEnemy = Instantiate(enemyType, Camera.main.ViewportToWorldPoint(spawnPosition,0), Quaternion.identity);
            newEnemy.GetComponent<Enemy>().player = GetComponent<Mastermind>().player.transform;
        }
        spawnRate += spawnRateIncrease*Time.deltaTime;

        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        if (transform.position.y < min.y)
        {
            Destroy(gameObject);
        }


    }
 
}
