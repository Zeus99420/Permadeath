using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteShooter : Enemy
{
    bool leftOrRight;
    public float flyByDistance;
    Vector2 targetPosition;
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
            if (leftOrRight)
            {
                targetPosition = (Vector2)player.position + Vector2.Perpendicular(playerDirection) * flyByDistance;

            }

            else targetPosition = (Vector2)player.position - Vector2.Perpendicular(playerDirection) * flyByDistance;
            direction = (targetPosition - (Vector2)transform.position).normalized;

        }
    }

}
