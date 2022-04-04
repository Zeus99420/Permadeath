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
            
            // V�ljer slumpm�ssigt en kant av sk�rmen, och sedan en slumpvald punkt strax utanf�r kanten d�r fienden ska spawna.
            spawnPosition = new Vector2(1.2f, Random.Range(1.2f, 2.4f));

            //for (int i = 0; i < maxPlanets; i++)
           // {
                GameObject planet = (GameObject)Instantiate(Planet, Camera.main.ViewportToWorldPoint(spawnPosition, 0), Quaternion.identity);
                planet.transform.localScale *= Random.Range(1f, 4f);
                planet.GetComponent<Planet>().speed = -(0.25f * Random.value + 0.05f);
                planet.transform.parent = transform;

                nextPlanetTime = Time.time + Random.Range(minCooldown, maxCooldown);
           // }
            

            
           
        }
    }


}