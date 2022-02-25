using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : Enemy
{
    public GameObject Mastermind;
    // public Transform player;
    Rigidbody2D m_rigidbody;
    public float acceleration;

    Vector2 direction = Vector2.up;
    float distance;

    public float attackRange;
    public float cooldown;
    float nextShotTime;

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
            distance = Vector2.Distance(player.position, transform.position); 
            direction = ((Vector2)player.position - (Vector2)transform.position).normalized;
            transform.up = direction;
            if (distance < attackRange)
            {
                direction = Vector2.Perpendicular(direction);
                if (Time.time > nextShotTime)
                {
                    nextShotTime = Time.time + cooldown;

                    GameObject newProjectile = Instantiate(projectile, transform.position, transform.rotation);
                    newProjectile.GetComponent<Rigidbody2D>().velocity = projectileSpeed * transform.up;

                }
            }
        }
        m_rigidbody.AddForce(direction * acceleration);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
}
