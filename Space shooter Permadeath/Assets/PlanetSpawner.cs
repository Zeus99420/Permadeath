using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetSpawner : MonoBehaviour
{
    public GameObject Planet;
    public int maxPlanet;
    Color[] planetColor =
    {
        new Color(0.5f, 0.5f, 1f),
        new Color(0, 1f, 1f),
        new Color(1f, 0f, 0f)
    };
    void Start()
    {
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
        for (int i = 0; i < maxPlanet; i++)
        {
            GameObject planet = (GameObject)Instantiate(Planet);
            planet.GetComponent<SpriteRenderer>().color = planetColor[i % planetColor.Length];
            planet.transform.position = new Vector2(Random.Range(min.x, max.x), Random.Range(min.y, max.y));
            planet.GetComponent<Planet>().speed = -(1f * Random.value + 0.05f);
            planet.transform.parent = transform;
        }
    }


    void Update()
    {

    }
}
