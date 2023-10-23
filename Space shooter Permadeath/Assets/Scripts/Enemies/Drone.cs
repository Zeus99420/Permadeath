using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : StandardEnemy
{
    //Den simplaste fienden, Flyger helt enkelt rakt mot spelaren och f�rs�ker krocka med den

    void FixedUpdate()
    {


        //R�r sig mot spelaren
        if (player)
        {
            direction = ((Vector2)player.position - (Vector2)transform.position).normalized;
            transform.up = direction;
        }
        AvoidCollision();
        m_rigidbody.AddForce(direction * acceleration);

    }




}
