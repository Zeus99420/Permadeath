using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoutShooter : Enemy
{
    public float acceleration;

    bool leftOrRight;
    public float flyByDistance;
    Vector2 targetPosition;

    float distanceToPlayer;
    public float attackRange;
    public float cooldown;
    float nextShotTime;

    public GameObject projectile;
    public float projectileSpeed;
    public int projectileDamage;
    public override void Start()
    {
        base.Start();

        if (Random.value < 0.5) leftOrRight = true;
        else leftOrRight = false;
    }

    void FixedUpdate()
    {
        AvoidCollision();

        if (player)
        {
            Vector2 playerDirection = ((Vector2)player.position - (Vector2)transform.position).normalized;
            transform.up = playerDirection;

            if (leftOrRight) targetPosition = (Vector2)player.position + Vector2.Perpendicular(playerDirection) * flyByDistance;
            else targetPosition = (Vector2)player.position - Vector2.Perpendicular(playerDirection) * flyByDistance;
            targetPosition -= playerDirection * flyByDistance;

            direction = (targetPosition - (Vector2)transform.position).normalized;

            // Skjuter på spelaren om den är inom räckhåll. En viss tid måste passera innan fienden kan skjuta igen.

            distanceToPlayer = Vector2.Distance(player.position, transform.position);
            if (IsInScreen(0.05f) && distanceToPlayer < attackRange && Time.time > nextShotTime)
            {
                nextShotTime = Time.time + cooldown;

                GameObject newProjectile = Instantiate(projectile, transform.position, transform.rotation, mastermind.projectilesContainer);
                newProjectile.GetComponent<Rigidbody2D>().velocity = projectileSpeed * transform.up;
                newProjectile.GetComponent<ShooterProjectile>().damage = projectileDamage;
            }

        }
        m_rigidbody.AddForce(direction * acceleration);
    }

}
