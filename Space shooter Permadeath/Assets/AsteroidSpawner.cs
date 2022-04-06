using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    public GameObject Asteroid;
    Vector2 spawnPosition;
    public float minCooldown;
    public float maxCooldown;
    float nextAsteroidTime = 0;

    private void Start()
    {
        
    }

    void Update()
    {

        if (Time.time > nextAsteroidTime)
        {
           float randomNumber = Random.value;
            // Väljer slumpmässigt en kant av skärmen, och sedan en slumpvald punkt strax utanför kanten där fienden ska spawna.
            if (randomNumber <= 0.2) spawnPosition = new Vector2(0.8f, Random.Range(1f, 0.8f));
            else if (randomNumber < 0.4) spawnPosition = new Vector2(1f, Random.Range(1f, 0.8f));
            else if (randomNumber < 0.7) spawnPosition = new Vector2(Random.Range(1f, 0.8f), 0.8f);
            else spawnPosition = new Vector2(Random.Range(1f, 0.8f), 1f);

            // Väljer slumpmässigt en kant av skärmen, och sedan en slumpvald punkt strax utanför kanten där fienden ska spawna.

            //spawnPosition = new Vector2(0.4f, Random.Range(0.4f, 5f));



            GameObject asteroid = (GameObject)Instantiate(Asteroid, Camera.main.ViewportToWorldPoint(spawnPosition, 0), Quaternion.identity);
            asteroid.transform.localScale *= Random.Range(0.5f, 2f);
           asteroid.GetComponent<Asteroid>().speed = -(3f * Random.value + 1f);
            asteroid.transform.parent = transform;

            nextAsteroidTime = Time.time + Random.Range(minCooldown, maxCooldown);
        }
    }
}
