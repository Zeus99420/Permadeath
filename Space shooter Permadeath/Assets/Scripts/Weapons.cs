using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Weapons : MonoBehaviour
{

    public float cooldown; //cooldownen innan spelaren kan skjuta igen (i sekunder)
    float nextShotTime = 0f; // Tiden n�r spelaren kan skjuta n�sta skott

    public GameObject projectile;
    public float projectileSpeed;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && Time.time > nextShotTime)
        {
            nextShotTime = Time.time + cooldown;    // Tidpunkten n�r spelare kan skjuta n�sta skott s�tts till nuvarande tid + cooldown

            GameObject newProjectile = Instantiate(projectile, transform.position, transform.rotation);
            newProjectile.GetComponent<Rigidbody2D>().velocity = projectileSpeed * transform.up;
        }

        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        if (transform.position.y < min.y)
        {
            Destroy(gameObject);
        }

    }
}
