using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : Enemy
{
    //Den simplaste fienden, Flyger helt enkelt rakt mot spelaren och f�rs�ker krocka med den
    public float acceleration;
    void FixedUpdate()
    {
        AvoidCollision();

        //R�r sig mot spelaren
        if (player)
        {
            direction = ((Vector2)player.position - (Vector2)transform.position).normalized;
        }
        m_rigidbody.AddForce(direction * acceleration);

    }




}
