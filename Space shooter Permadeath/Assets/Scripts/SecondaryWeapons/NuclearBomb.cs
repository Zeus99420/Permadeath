using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NuclearBomb : SecondaryWeapons
{
    public GameObject projectile;

    public int damage;
    public float delay;
    public float radius;

    public override void UseWeapon()
    {
        GameObject newProjectile = Instantiate(projectile, transform.position, transform.rotation, mastermind.stuffContainer);
        newProjectile.GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity;
        newProjectile.GetComponent<NuclearBombProjectile>().delay = delay;
        newProjectile.GetComponent<NuclearBombProjectile>().damage = damage;
        newProjectile.GetComponent<NuclearBombProjectile>().radius = radius;
    }
}
