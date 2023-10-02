using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketSwarm : SecondaryWeapons
{
    public GameObject projectile;

    public int damage;
    public int rocketCount;
    public float acceleration;
    public float engineDelay;

    public float launchVelocity;
    public float launchTime;
    public float spread;



    public override void UseWeapon()
    {
        StartCoroutine(LaunchSwarm());
    }

    IEnumerator LaunchSwarm()
    {
        Vector2 targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 interceptPos = targetPosition;
        float timeToReach;
        float distance;

        for (int i = 0; i < 10; i++)
        {
            distance = ((Vector2)transform.position - interceptPos).magnitude;
            timeToReach = Mathf.Sqrt(distance / (0.5f * acceleration));
            interceptPos = targetPosition - GetComponent<Rigidbody2D>().velocity * timeToReach;
        }

        Vector2 targetVector = ((Vector2)transform.position - interceptPos).normalized;




        // Rockets are launched in pairs flying in opposite directions, one to the left and one to the right.
        int rocketPairs = rocketCount / 2;
        float angle = spread / 2;
        float angleIncrement = spread / (rocketPairs-1);
        float velocityIncrement = 3f / (rocketPairs-1);
        float timeBetweenRockets = launchTime / (rocketPairs-1);
        float velocity = 4f;

        for (int i = 0; i < rocketPairs; i++)
        {
            Vector2 launchVector = Vector2.Perpendicular(targetVector) * velocity;
            launchVector = Quaternion.Euler(0, 0, angle) * launchVector;
            LaunchRocket(launchVector, targetPosition);
            launchVector = -Vector2.Perpendicular(targetVector) * velocity;
            launchVector = Quaternion.Euler(0, 0, -angle) * launchVector;
            LaunchRocket(launchVector, targetPosition);
            velocity -= velocityIncrement;
            angle -= angleIncrement;
            yield return new WaitForSeconds(timeBetweenRockets);
        }
    }

    void LaunchRocket (Vector2 launchVector, Vector2 targetPosition)
    {
        GameObject newProjectile = Instantiate(projectile, transform.position, transform.rotation, mastermind.stuffContainer);
        newProjectile.GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity + launchVelocity * launchVector;
        newProjectile.transform.up = launchVector;

        SwarmRocket rocket = newProjectile.GetComponent<SwarmRocket>();
        rocket.targetPosition = targetPosition;
        rocket.acceleration = acceleration;
        rocket.damage = (int)(damage * damageMultiplier);
        rocket.engineDelay = engineDelay;
    }
}
