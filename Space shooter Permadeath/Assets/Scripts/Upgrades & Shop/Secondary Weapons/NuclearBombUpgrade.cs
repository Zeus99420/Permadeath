using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NuclearBombUpgrade : SecondaryWeaponsUpgrade
{
    public GameObject projectile;

    public int maxDamage;
    public float delay;
    public float radius;
    public float explosionDuration;
    public override SecondaryWeapons BuySecondaryWeapon()
    {
        NuclearBomb nuke = player.AddComponent<NuclearBomb>();

        nuke.projectile = projectile;
        nuke.maxDamage = maxDamage;
        nuke.delay = delay;
        nuke.radius = radius;
        nuke.explosionDuration = explosionDuration;

        return nuke;
    }
}
