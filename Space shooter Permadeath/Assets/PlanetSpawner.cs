using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetSpawner : MonoBehaviour
{


    public GameObject Planet;
   [HideInInspector] public  Vector2 spawnPosition;
        public float spawnRate;
   // public int maxPlanets;




    

    void Update()
    {
        float randomNumber = Random.value;
        if (randomNumber < Time.deltaTime * spawnRate)
        {
            randomNumber = Random.value;
            // Väljer slumpmässigt en kant av skärmen, och sedan en slumpvald punkt strax utanför kanten där fienden ska spawna.
            if (randomNumber <= 0.2) spawnPosition = new Vector2(1.1f, Random.Range(-0.1f, 1.1f));
            else if (randomNumber < 0.4) spawnPosition = new Vector2(-0.1f, Random.Range(-0.1f, 1.1f));
            else if (randomNumber < 0.7) spawnPosition = new Vector2(Random.Range(-0.1f, 1.1f), 1.1f);
            else spawnPosition = new Vector2(Random.Range(-0.1f, 1.1f), -0.1f);
            //for (int i = 0; i < maxPlanets; i++)
           // {
                GameObject planet = (GameObject)Instantiate(Planet, Camera.main.ViewportToWorldPoint(spawnPosition, 0), Quaternion.identity);
                //planet.GetComponent<SpriteRenderer>().color = Color[i % Color.Length];
                planet.transform.position = new Vector2(Random.Range(-4, -8), Random.Range(-4, -8));
                planet.transform.localScale = new Vector2( Random.Range(1, 3), Random.Range(1, 3));
                planet.GetComponent<Planet>().speed = -(0.25f * Random.value + 0.05f);
                planet.transform.parent = transform;

           // }
            spawnRate -= Time.deltaTime;

            Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
            if (transform.position.y < min.y)
            {
                Destroy(gameObject);
            }
        }
    }


}