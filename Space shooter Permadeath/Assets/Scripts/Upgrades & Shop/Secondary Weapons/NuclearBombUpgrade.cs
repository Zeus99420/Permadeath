using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NuclearBombUpgrade : SecondaryWeaponsUpgrade
{
    public GameObject projectile;

    public int maxDamage;
    public float friendlyDamageMultiplier;
    public float delay;
    public float radius;
    public float explosionDuration;
    public float launchVelocity;

    public override SecondaryWeapons BuySecondaryWeapon()
    {
        NuclearBomb nuke = player.AddComponent<NuclearBomb>();

        nuke.projectile = projectile;
        nuke.maxDamage = maxDamage;
        nuke.friendlyDamageMultiplier = friendlyDamageMultiplier;
        nuke.delay = delay;
        nuke.radius = radius;
        nuke.explosionDuration = explosionDuration;
        nuke.launchVelocity = launchVelocity;
        

        return nuke;
    }

    public override string GetDescription()
    {
        return
        ("Launch a bomb which explodes after "+ delay +" seconds, dealing damage in a large radius." +
        "\n\nThe bomb <b>can damage you</b> as well as enemies. " +
        "\n\nDamage is greatest at the center of the explosion and decreases with distance." +
        "\n\nThe bomb can be aimed but is affected by your velocity when launching it."

        //"\n\nmax damage: " + maxDamage +
        //"\n radius: " + radius



        ) ;
    }
}
