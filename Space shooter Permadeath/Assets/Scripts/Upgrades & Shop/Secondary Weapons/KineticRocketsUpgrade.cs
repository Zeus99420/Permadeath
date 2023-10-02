using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KineticRocketsUpgrade : SecondaryWeaponsUpgrade
{
    public GameObject projectile;

    public float projectileAcceleration;
    public float damageMultiplier;

    public override SecondaryWeapons BuySecondaryWeapon()
    {
        KineticRockets rockets = player.AddComponent<KineticRockets>();

        rockets.projectile = projectile;
        rockets.projectileAcceleration = projectileAcceleration;
        rockets.damage = damageMultiplier;
        return rockets;
    }
}
