using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invader : Enemy
{
    public GameObject projectile;
    float distanceToPlayer;
    public float attackRange;

    float nextShotTime;
    public float cooldown;
    public float projectileSpeed;
    public int projectileDamage;
    public Transform weapon;

    private void FixedUpdate()
    {


        if (player)
        {
            distanceToPlayer = Vector2.Distance(player.position, transform.position);
            direction = ((Vector2)player.position - (Vector2)transform.position).normalized;
            transform.up = direction;

            if (IsInScreen(0.05f) && distanceToPlayer < attackRange && Time.time > nextShotTime)
            {
                nextShotTime = Time.time + cooldown;

                GameObject newProjectile = Instantiate(projectile, weapon.position+ transform.up*0.2f, transform.rotation, mastermind.stuffContainer);
                newProjectile.GetComponent<Rigidbody2D>().velocity = projectileSpeed * transform.up;
                newProjectile.GetComponent<EnemyProjectile>().damage = projectileDamage;

            }
        }
        AvoidCollision();
        m_rigidbody.AddForce(direction * acceleration);
    }
      
    


}
