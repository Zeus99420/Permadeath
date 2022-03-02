using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : Enemy
{
    //En fiende som försöker närma sig spelaren och skjuta den med projektiler

    public float acceleration;

    float distanceToPlayer;

    public float attackRange;
    public float cooldown;
    float nextShotTime;

    public float strafeRange;

    public GameObject projectile;
    public float projectileSpeed;
    public int projectileDamage;



    // Update is called once per frame
    void FixedUpdate()
    {
        if (player)
        {
            distanceToPlayer = Vector2.Distance(player.position, transform.position); 
            direction = ((Vector2)player.position - (Vector2)transform.position).normalized;
            transform.up = direction;

            //Fienden rör sig vanligtvis mot spelaren tills den kommer inom ett visst avstånd. Då rör den sig istället i sidled runt spelaren
            if (distanceToPlayer < strafeRange) direction = Vector2.Perpendicular(direction);



            // Skjuter på spelaren om den är inom räckhåll. En viss tid måste passera innan fienden kan skjuta igen.
            if (IsInScreen() && distanceToPlayer < attackRange && Time.time > nextShotTime)
            {
               nextShotTime = Time.time + cooldown;

               GameObject newProjectile = Instantiate(projectile, transform.position, transform.rotation);
               newProjectile.GetComponent<Rigidbody2D>().velocity = projectileSpeed * transform.up;
               newProjectile.GetComponent<ShooterProjectile>().damage = projectileDamage; 
            }
        }
        m_rigidbody.AddForce(direction * acceleration);

    }



}
