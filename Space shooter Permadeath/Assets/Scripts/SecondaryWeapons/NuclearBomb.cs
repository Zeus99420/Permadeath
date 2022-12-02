using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NuclearBomb : SecondaryWeapons
{
    public GameObject projectile;

    public int maxDamage;
    public float delay;
    public float radius;
    public float explosionDuration;

    public override void UseWeapon()
    {
        GameObject newProjectile = Instantiate(projectile, transform.position, transform.rotation, mastermind.stuffContainer);
        newProjectile.GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity;
        Explosive explosive = newProjectile.GetComponent<Explosive>();
        explosive.explosionDuration = explosionDuration;
        explosive.maxDamage = maxDamage;
        explosive.radius = radius;

        StartCoroutine(explosive.Countdown(delay));
    }
}
