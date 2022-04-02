using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Railgun : Upgrades
{
    public float projectileSpeedIncrease;
    public float rateOfFireMultiplier;
    public float damageMultiplier;
    public GameObject projectile;
    public override void Buy()
    {
        Weapons weapons = player.GetComponent<Weapons>();
        weapons.projectileSpeed += projectileSpeedIncrease;
        weapons.baseDamage = (int)(weapons.baseDamage*damageMultiplier);
        weapons.projectile = projectile;
        weapons.rateOfFire *= rateOfFireMultiplier;
        weapons.rateOfFireMultiplier *= rateOfFireMultiplier;
    }
}
