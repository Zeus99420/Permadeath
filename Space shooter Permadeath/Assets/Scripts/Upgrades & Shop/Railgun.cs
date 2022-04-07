using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Railgun : Upgrades
{
    public float projectileSpeedIncrease;
    public float rateOfFireMultiplier;
    public float damageMultiplier;
    public GameObject projectile;
    public float projectileSize;


    public override void Buy()
    {
        Weapons weapons = player.GetComponent<Weapons>();
        weapons.projectileSpeed += projectileSpeedIncrease;
        weapons.baseDamage = (int)(weapons.baseDamage*damageMultiplier);
        weapons.projectile = projectile;
        weapons.rateOfFire *= rateOfFireMultiplier;
        weapons.rateOfFireMultiplier *= rateOfFireMultiplier;
        LineRenderer lineRenderer = player.GetComponent<LineRenderer>();
        Vector3 newPosition = lineRenderer.GetPosition(1) + Vector3.up * 4;
        lineRenderer.SetPosition(1, newPosition);
        weapons.projectileSize *= projectileSize;
    }

    public override string GetDescription()
    {
        return ("Your projectiles fly faster and can pierce and hit multiple enemies. Damage is halved after each hit." +
            "\n\ndamage: x" + damageMultiplier +
            "\nrate of fire: x" + rateOfFireMultiplier
            );
    }
}
