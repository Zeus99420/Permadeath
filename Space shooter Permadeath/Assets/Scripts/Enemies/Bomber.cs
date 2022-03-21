using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomber : Enemy
{
    public float flyByDistance;
    public float acceleration;

    public GameObject projectile;
    public int projectileDamage;
    public float projectileSpeed;
    public float minProjectileSpeed;
    public float maxProjectileSpeed;
    public float minCooldown;
    public float maxCooldown;
    public float projectileSpread;

    public float nextShotTime;


    
    Vector2 bombDirection;

    public override void Start()
    {
        base.Start();
        FlyBy();

    }
   void FlyBy()
    {
        Vector2 playerDirection = ((Vector2)player.position - (Vector2)transform.position).normalized;
        Vector2 targetPosition;
        //En position, antingen till höger eller vänster om spelaren väljs
        //Fienden flyger genom punkten, dvs förbi spelaren, och släpper bomber
        if (Random.value < 0.5)
        {
            targetPosition = (Vector2)player.position + Vector2.Perpendicular(playerDirection) * flyByDistance;
            direction = (targetPosition - (Vector2)transform.position).normalized;
            bombDirection = -Vector2.Perpendicular(direction);
        }
        else
        {
            targetPosition = (Vector2)player.position - Vector2.Perpendicular(playerDirection) * flyByDistance;
            direction = (targetPosition - (Vector2)transform.position).normalized;
            bombDirection = Vector2.Perpendicular(direction);
        }

        transform.up = direction;
    }

    void FixedUpdate()
    {
        AvoidCollision();
        m_rigidbody.AddForce(direction * acceleration);
    }

    private void Update()
    {
        if (IsInScreen() && Time.time > nextShotTime)
        {
            nextShotTime = Time.time + Random.Range(minCooldown, maxCooldown);

            GameObject newProjectile = Instantiate(projectile, transform.position, transform.rotation);
            Rigidbody2D projectileRigidbody = newProjectile.GetComponent<Rigidbody2D>();
            Vector2 randomVector = new Vector2(Random.Range(-projectileSpread, projectileSpread),
                Random.Range(-projectileSpread, projectileSpread));
            projectileRigidbody.velocity = projectileSpeed * bombDirection + randomVector;
            newProjectile.GetComponent<ShooterProjectile>().damage = projectileDamage;
        }
    }


    //När bombern lämnar skärmen så vänder den tillbaka och flyger förbi igen 
    private void OnBecameInvisible()
    {
        FlyBy();
    }
}
