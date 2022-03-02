using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : Enemy
{
    //Den simplaste fienden, Flyger helt enkelt rakt mot spelaren och f�rs�ker krocka med den
    public GameObject Mastermind;
    public float acceleration;



    // Update is called once per frame
    void FixedUpdate()
    {
        //R�r sig mot spelaren
        if (player)
        {
            direction = ((Vector2)player.position - (Vector2)transform.position).normalized;
        }

        // AvoidCollision();

        m_rigidbody.AddForce(direction * acceleration);

    }




}
