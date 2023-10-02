using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KineticRockets : SecondaryWeapons
{
    public GameObject projectile;

    public float projectileAcceleration;
    public float damage;

    public override void UseWeapon()
    {
        GameObject newProjectile = Instantiate(projectile, transform.position, transform.rotation, mastermind.stuffContainer);
        newProjectile.GetComponent<KineticRocketProjectile>().acceleration = projectileAcceleration;
        newProjectile.GetComponent<KineticRocketProjectile>().damageMultiplier = damage * damageMultiplier;
    }


 




}
