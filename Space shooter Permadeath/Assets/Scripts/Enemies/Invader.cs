using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invader : Enemy
{
    public GameObject projectile;
    float distanceToPlayer;
    public float attackRange;
    public float acceleration;
    float nextShotTime;
    public float cooldown;
    public float projectileSpeed;
    public int projectileDamage;

    private void FixedUpdate()
    {
        AvoidCollision();

        if (player)
        {
            distanceToPlayer = Vector2.Distance(player.position, transform.position);
            direction = ((Vector2)player.position - (Vector2)transform.position).normalized;
            transform.up = direction;

            if (IsInScreen(0.05f) && distanceToPlayer < attackRange && Time.time > nextShotTime)
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
