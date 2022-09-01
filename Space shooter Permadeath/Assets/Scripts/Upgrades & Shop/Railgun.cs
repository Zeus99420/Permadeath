using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Railgun : Upgrades
{
    public float projectileSpeedIncrease;
    public float rateOfFireMultiplier;
    public float damageMultiplier;
    public float projectileSize;
    public float piercingMultiplier;


    public override void Buy()
    {
        Weapons weapons = player.GetComponent<Weapons>();
        weapons.projectileSpeed += projectileSpeedIncrease;
        weapons.baseDamage = (int)(weapons.baseDamage*damageMultiplier);
        weapons.rateOfFire *= rateOfFireMultiplier;
        weapons.rateOfFireMultiplier *= rateOfFireMultiplier;
        LineRenderer lineRenderer = player.GetComponent<LineRenderer>();
        Vector3 newPosition = lineRenderer.GetPosition(1) + Vector3.up * 4;
        lineRenderer.SetPosition(1, newPosition);
        weapons.projectileSize *= projectileSize;
        weapons.piercing = true;
        weapons.piercingMultiplier = piercingMultiplier;
    }

    public override string GetDescription()
    {
        return ("Your projectiles fly faster and can pierce and hit multiple enemies. Damage is reduced after each hit." +
            "\n\nDamage: x" + damageMultiplier +
            "\nRate of Fire: x" + rateOfFireMultiplier
            );
    }
}
