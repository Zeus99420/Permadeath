using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileUpgrade : SecondaryWeaponsUpgrade
{
    public GameObject projectile;

    public float acceleration;
    public int maxDamage;
    public float radius;
    public float explosionDuration;
    public override SecondaryWeapons BuySecondaryWeapon()
    {
        FragMissile missile = player.AddComponent<FragMissile>();
        missile.projectile = projectile;
        missile.maxDamage = maxDamage;
        missile.acceleration = acceleration;
        missile.radius = radius;
        missile.explosionDuration = explosionDuration;
        return missile;
    }
}
