using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : Enemy
{
    //En fiende som f�rs�ker n�rma sig spelaren och skjuta den med projektiler

    public GameObject Mastermind;
    Rigidbody2D m_rigidbody;
    public float acceleration;

    float distanceToPlayer;

    public float attackRange;
    public float cooldown;
    float nextShotTime;

    public float strafeRange;

    public GameObject projectile;
    public float projectileSpeed;

    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (player)
        {
            distanceToPlayer = Vector2.Distance(player.position, transform.position); 
            direction = ((Vector2)player.position - (Vector2)transform.position).normalized;
            transform.up = direction;

            //Fienden r�r sig vanligtvis mot spelaren tills den kommer inom ett visst avst�nd. D� r�r den sig ist�llet i sidled runt spelaren
            if (distanceToPlayer < strafeRange) direction = Vector2.Perpendicular(direction);



            // Skjuter p� spelaren om den �r inom r�ckh�ll. En viss tid m�ste passera innan fienden kan skjuta igen.
            if (IsInScreen() && distanceToPlayer < attackRange && Time.time > nextShotTime)
            {
               nextShotTime = Time.time + cooldown;

               GameObject newProjectile = Instantiate(projectile, transform.position, transform.rotation);
               newProjectile.GetComponent<Rigidbody2D>().velocity = projectileSpeed * transform.up;               
            }
        }
        m_rigidbody.AddForce(direction * acceleration);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(collision.gameObject);
        }
    }

}
