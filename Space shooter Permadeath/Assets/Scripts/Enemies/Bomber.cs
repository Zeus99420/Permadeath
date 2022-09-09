using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomber : Enemy
{
    public float flyByDistance;

    public GameObject projectile;
    public int projectileDamage;
    public float projectileSpeed;
    public float minCooldown;
    public float maxCooldown;
    public float projectileSpread;

    float nextShotTime;

    bool leftOrRight;
    bool approaching;


    
    Vector2 bombDirection;
    Vector2 targetPosition;


    public override void Start()
    {
        base.Start();
        Approach();

    }
   void Approach()
    {
        approaching = true;
        //En position, antingen till höger eller vänster om spelaren väljs
        //Fienden flyger genom punkten, dvs förbi spelaren, och släpper bomber
        if (Random.value < 0.5) leftOrRight = true;
        else  leftOrRight = false;

    }

    void FixedUpdate()
    {
        if (player)
        {
            if (approaching)
            {
                Vector2 playerDirection = ((Vector2)player.position - (Vector2)transform.position).normalized;
                if (leftOrRight)
                {
                    targetPosition = (Vector2)player.position + Vector2.Perpendicular(playerDirection) * flyByDistance;
                    targetPosition -= playerDirection * flyByDistance;
                    direction = (targetPosition - (Vector2)transform.position).normalized;
                    bombDirection = -Vector2.Perpendicular(direction);
                    
                }

                else
                {
                    targetPosition = (Vector2)player.position - Vector2.Perpendicular(playerDirection) * flyByDistance;
                    targetPosition -= playerDirection * flyByDistance;
                    direction = (targetPosition - (Vector2)transform.position).normalized;
                    bombDirection = Vector2.Perpendicular(direction);
                }
                transform.up = direction;
                if (IsInScreen(0.1f)) { approaching = false; }
            }

            else if (!IsInScreen(-0.1f)) Approach();
        }

        direction = transform.up;
        AvoidCollision();
        m_rigidbody.AddForce(direction * acceleration);
    }

    private void Update()
    {
        if (IsInScreen(0) && Time.time > nextShotTime)
        {
            nextShotTime = Time.time + Random.Range(minCooldown, maxCooldown);

            GameObject newProjectile = Instantiate(projectile, transform.position, transform.rotation, mastermind.stuffContainer);
            Rigidbody2D projectileRigidbody = newProjectile.GetComponent<Rigidbody2D>();
            Vector2 randomVector = new Vector2(Random.Range(-projectileSpread, projectileSpread),
                Random.Range(-projectileSpread, projectileSpread));
            projectileRigidbody.velocity = /*m_rigidbody.velocity*0.5f*/ + projectileSpeed * bombDirection + randomVector;
            newProjectile.GetComponent<EnemyBomb>().damage = projectileDamage;
            newProjectile.GetComponent<EnemyBomb>().mastermind = mastermind;
        }
    }


    //För Debugging
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(targetPosition, 0.2f);
    }

}
