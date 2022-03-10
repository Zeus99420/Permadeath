using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Weapons : MonoBehaviour
{

    public float rateOfFire; //Antal skott spelaren kan avfyra per sekund
    float nextShotTime = 0f; // Tiden när spelaren kan skjuta nästa skott

    public GameObject projectile;
    public float projectileSpeed;
    public int projectileDamage;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && Time.time > nextShotTime)
        {
            nextShotTime = Time.time + 1/rateOfFire;    // Sätter en tidpunkt när spelaren kan avfyra igen

            GameObject newProjectile = Instantiate(projectile, transform.position, transform.rotation);
            newProjectile.GetComponent<Rigidbody2D>().velocity = projectileSpeed * transform.up;
           newProjectile.GetComponent<PlayerProjectile>().damage = projectileDamage;
        }

        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        if (transform.position.y < min.y)
        {
            Destroy(gameObject);
        }

    }
}
