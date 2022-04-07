using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetSpawner : MonoBehaviour
{


    public GameObject Planet;
   Vector2 spawnPosition;
        

    public float minCooldown;
    public float maxCooldown;
    float nextPlanetTime = 0;
   // public int maxPlanets;






    void Update()
    {
        
        if (Time.time > nextPlanetTime)
        {
            if (Random.value < 0.4f) spawnPosition = new Vector2(1.5f, Random.Range(0.8f, 1.3f));
            else spawnPosition = new Vector2(Random.Range(0.8f, 1.3f),1.5f);

            // Väljer slumpmässigt en kant av skärmen, och sedan en slumpvald punkt strax utanför kanten där fienden ska spawna.
            //spawnPosition = new Vector2(1.2f, Random.Range(1.2f, 2.4f));

            
                GameObject planet = (GameObject)Instantiate(Planet, Camera.main.ViewportToWorldPoint(spawnPosition, 0), Quaternion.identity);
                planet.transform.localScale *= Random.Range(1f, 4f);
                planet.GetComponent<Planet>().speed = -(0.25f * Random.value + 0.05f);
                planet.transform.parent = transform;

                nextPlanetTime = Time.time + Random.Range(minCooldown, maxCooldown);
         
            

            
           
        }
    }


}