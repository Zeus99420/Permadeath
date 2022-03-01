using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invader : Enemy
{
    public GameObject Mastermind;
    public GameObject projectile;
    Rigidbody2D m_rigidbody;
    float distanceToPlayer;
    public float attackRange;
    public float acceleration;
    float nextShotTime;
    public float cooldown;
    public float projectileSpeed;
    // Start is called before the first frame update
    void Start()
    {
            m_rigidbody = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        if (player)
        {
            distanceToPlayer = Vector2.Distance(player.position, transform.position);
            direction = ((Vector2)player.position - (Vector2)transform.position).normalized;
            transform.up = direction;

            if (IsInScreen() && distanceToPlayer < attackRange && Time.time > nextShotTime)
            {
                nextShotTime = Time.time + cooldown;

                GameObject newProjectile = Instantiate(projectile, transform.position, transform.rotation);
                newProjectile.GetComponent<Rigidbody2D>().velocity = projectileSpeed * transform.up;
            }
        }
        m_rigidbody.AddForce(direction * acceleration);
    }
      
    
    // Update is called once per frame
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(collision.gameObject);
            
        }
    }

}
