using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FragMissile : SecondaryWeapons
{

    public GameObject projectile;
    /*[HideInInspector]*/ public float acceleration;

    //public int shrapnelCount;
    //public int shrapnelDamage;
    //public float shrapnelVelocity;

    public int maxDamage;
    public float radius;
    public float explosionDuration;

    float distance;
    float timeToReach;
    public override void UseWeapon()
    {
        Vector2 targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 interceptPos = targetPos;

        for (int i=0; i<10; i++)
        {
            distance = ((Vector2)transform.position - interceptPos).magnitude;
            timeToReach = Mathf.Sqrt(distance / (0.5f * acceleration));
            interceptPos = targetPos - GetComponent<Rigidbody2D>().velocity * timeToReach;

        }

        GameObject newProjectile = Instantiate(projectile, transform.position, transform.rotation, mastermind.stuffContainer);
        newProjectile.GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity;
        newProjectile.transform.up = (interceptPos - (Vector2)transform.position).normalized;
        FragMissileProjectile fragMissileProjectile = newProjectile.GetComponent<FragMissileProjectile>();
        fragMissileProjectile.mastermind = mastermind;
        fragMissileProjectile.acceleration = acceleration;
        fragMissileProjectile.targetPosition = targetPos;

        fragMissileProjectile.explosionDuration = explosionDuration;
        fragMissileProjectile.maxDamage = (int)(maxDamage * damageMultiplier);
        fragMissileProjectile.radius = radius * radiusMultiplier;

        //fragMissileProjectile.shrapnelCount = shrapnelCount;
        //fragMissileProjectile.shrapnelDamage = shrapnelDamage;
        //fragMissileProjectile.shrapnelVelocity = shrapnelVelocity;
        fragMissileProjectile.countdown = fragMissileProjectile.StartCoroutine(fragMissileProjectile.Countdown(timeToReach));

    }
}
