using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpeed : Upgrades
{
    public float increaseAmount;
    public override void Buy()
    {
        Weapons weapons = player.GetComponent<Weapons>();
        weapons.projectileSpeed += increaseAmount * weapons.projectileSpeedMultiplier;
    }


    public override string GetDescription()
    {
        Weapons weapons = player.GetComponent<Weapons>();
        float newValue = (weapons.projectileSpeed + increaseAmount * weapons.projectileSpeedMultiplier);
        return ("Increases the velocity of your gun's projectiles." +
            "\n\nProjectile Speed : " + weapons.projectileSpeed + " -> " + newValue
            );
    }
}
