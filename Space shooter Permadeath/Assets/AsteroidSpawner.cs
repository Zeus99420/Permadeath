using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    public GameObject Asteroid;
    public GameObject Background;
    public float spawn_circle_radius;
    public int asteroid_count;
    public int asteroid_limit;
    public int asteroids_per_frame;
    public float fastest_speed;
    public float slowest_speed;


    private void Start()
    {
        
    }

    private void Update()
    {
        MaintainPopulation();
    }

    void MaintainPopulation()
    {
        if(asteroid_count < asteroid_limit)
        {
            for(int i=0; i <asteroids_per_frame; i++)
            {
                Vector3 position = GetRandomPosition();
                Asteroid asteroid_script = AddAsteroid(position);
                asteroid_script.transform.Rotate(Vector3.forward * Random.Range(-45.0f, 45.0f));
            }
        }
    }

    Vector3 GetRandomPosition()
    {
        Vector3 position = Random.insideUnitCircle.normalized;
        position *= spawn_circle_radius;
        position += Background.transform.position;
        return position;
    }

  Asteroid AddAsteroid (Vector3 position)
    {
        asteroid_count += 1;
        GameObject new_asteroid = Instantiate(
            Asteroid,
            position,
            Quaternion.FromToRotation(Vector3.up, (Background.transform.position - position)),
            gameObject.transform
            );
        //gameObject.transform.localScale *= Random.Range(1f, 0.5f);
        Asteroid asteroid_script = new_asteroid.GetComponent<Asteroid>();
        asteroid_script.asteroid_spawner = this;
        asteroid_script.Background = Background;
        asteroid_script.speed = Random.Range(slowest_speed, fastest_speed);

        return asteroid_script;
    }
    /*
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
    */
}
