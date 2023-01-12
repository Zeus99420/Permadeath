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


        int rocketPairs = rocketCount / 2;
        float angle = spread / 2;
        float angleIncrement = spread / (rocketPairs-1);
        float velocityIncrement = 3f / (rocketPairs-1);
        float timeBetweenRockets = launchTime / (rocketPairs-1);
        float velocity = 4f;
  

        //float angle = -spread / 2;
        //float angleIncrement = spread / (spreadBulletCount - 1);

        // Rockets are launched in pairs flying in opposite directions, one to the left and one to the right

        for (int i = 0; i < rocketPairs; i++)
        {
            Vector2 launchVector = transform.right * velocity;
            launchVector = Quaternion.Euler(0, 0, angle) * launchVector;
            LaunchRocket(launchVector, targetPosition);
            launchVector = -transform.right * velocity;
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
        //newProjectile.transform.up = launchVector;

        SwarmRocket rocket = newProjectile.GetComponent<SwarmRocket>();
        rocket.targetPosition = targetPosition;
        rocket.acceleration = acceleration;
        rocket.damage = damage;
        rocket.engineDelay = engineDelay;
    }
}
