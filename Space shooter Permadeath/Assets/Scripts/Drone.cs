using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : Enemy
{
    //Den simplaste fienden, Flyger helt enkelt rakt mot spelaren och försöker krocka med den
    public GameObject Mastermind;
    Rigidbody2D m_rigidbody;
    public float acceleration;

    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Rör sig mot spelaren
        if (player)
        {
            direction = ((Vector2)player.position - (Vector2)transform.position).normalized;
        }

        // AvoidCollision();

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
