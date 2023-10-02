using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NuclearBomb : SecondaryWeapons
{
    public GameObject projectile;

    public int maxDamage;
    public float friendlyDamageMultiplier;
    public float delay;
    public float radius;
    public float explosionDuration;
    public float launchVelocity;


    public override void UseWeapon()
    {

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 targetVector = mousePos - (Vector2)transform.position;
        if (targetVector.magnitude > 5) targetVector = targetVector.normalized * 5;
        targetVector /= 5;





        GameObject newProjectile = Instantiate(projectile, transform.position, transform.rotation, mastermind.stuffContainer);
        newProjectile.GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity + launchVelocity * targetVector;
        Explosive explosive = newProjectile.GetComponent<Explosive>();
        explosive.explosionDuration = explosionDuration;
        explosive.maxDamage = (int)(maxDamage * damageMultiplier);
        explosive.radius = radius * radiusMultiplier;
        explosive.friendlyDamageMultiplier = friendlyDamageMultiplier;

        explosive.countdown = explosive.StartCoroutine(explosive.Countdown(delay));
    }
}
