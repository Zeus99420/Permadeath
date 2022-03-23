using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigBullet : Upgrades
{
    public GameObject projectile;
    public float projectileSpeed;

    public override void BuyFirst()
    {
        Weapons weapons = player.GetComponent<Weapons>();
        weapons.fireMode = "FireBigSlowBullet";
        //weapons.projectileSpeed = projectileSpeed / 4;
        //GameObject playerprojectile = Instantiate(projectile, transform.position, transform.rotation);
        //playerprojectile.GetComponent<PlayerProjectile>();
        //playerprojectile.transform.localScale *= 5.2f;

    }

    public override void BuyAnother()
    {
        Weapons weapons = player.GetComponent<Weapons>();
    }

}


