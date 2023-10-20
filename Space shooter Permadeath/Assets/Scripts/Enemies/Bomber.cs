using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomber : Enemy
{
    public float flyByDistance;
    public float turnRate;

    public GameObject projectile;
    public int projectileDamage;
    public float projectileSpeed;
    public float minCooldown;
    public float maxCooldown;
    public float projectileSpread;

    float nextShotTime;

    float leftRight;
    bool approaching;


    
    Vector2 targetPosition;
    Vector2 runDirection;


    public override void Start()
    {
        base.Start();
        Approach();

    }
   void Approach()
    {
        approaching = true;
        if (Random.value < 0.5) leftRight = -1;
        else  leftRight = 1;

    }

    void FixedUpdate()
    {
        if (player)
        {
            if (approaching)
            {
                Vector2 playerDirection = ((Vector2)player.position - (Vector2)transform.position).normalized;

                targetPosition = (Vector2)player.position + leftRight * Vector2.Perpendicular(playerDirection) * flyByDistance;
                targetPosition -= playerDirection * flyByDistance;
                direction = (targetPosition - (Vector2)transform.position).normalized;
                    
                //transform.up = direction;
                if (IsInScreen(0.1f))
                {
                    approaching = false;
                    runDirection = direction;
                }
            }

            else
            {
                direction = runDirection;
                if (!IsInScreen(-0.1f)) Approach();
            }
        }

        //direction = transform.up;
        AvoidCollision();
        transform.up = Vector3.Slerp(transform.up, direction, turnRate * Time.fixedDeltaTime);
        m_rigidbody.AddForce(transform.up * acceleration);
    }

    private void Update()
    {
        if (IsInScreen(0) && Time.time > nextShotTime)
        {
            nextShotTime = Time.time + Random.Range(minCooldown, maxCooldown);

            Vector2 bombDirection = transform.right * leftRight; 
            GameObject newProjectile = Instantiate(projectile, transform.position, transform.rotation, mastermind.stuffContainer);
            Rigidbody2D projectileRigidbody = newProjectile.GetComponent<Rigidbody2D>();
            Vector2 randomVector = new Vector2(Random.Range(-projectileSpread, projectileSpread),
                Random.Range(-projectileSpread, projectileSpread));
            projectileRigidbody.velocity = /*m_rigidbody.velocity*0.5f*/ + projectileSpeed * bombDirection + randomVector;
            newProjectile.GetComponent<EnemyBomb>().damage = projectileDamage;
            newProjectile.GetComponent<EnemyBomb>().mastermind = mastermind;
        }
    }

}
