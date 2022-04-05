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

    void Update()
    {

        if (Time.time > nextAsteroidTime)
        {

            // Väljer slumpmässigt en kant av skärmen, och sedan en slumpvald punkt strax utanför kanten där fienden ska spawna.
            //spawnPosition = new Vector2(0.4f, Random.Range(0.4f, 5f));
            Vector2 targetPosition;
            targetPosition.x = Random.Range(0.2f, 0.8f);
            targetPosition.y = Random.Range(0.2f, 0.8f);
            targetPosition = Camera.main.ViewportToWorldPoint(targetPosition);
            Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
            transform.up = direction;


            GameObject asteroid = (GameObject)Instantiate(Asteroid, Camera.main.ViewportToWorldPoint(spawnPosition, 0), Quaternion.identity);
            asteroid.transform.localScale *= Random.Range(0.5f, 2f);
            asteroid.GetComponent<Asteroid>().speed = -(3f * Random.value + 1f);
            asteroid.transform.parent = transform;

            nextAsteroidTime = Time.time + Random.Range(minCooldown, maxCooldown);
        }
    }
}
