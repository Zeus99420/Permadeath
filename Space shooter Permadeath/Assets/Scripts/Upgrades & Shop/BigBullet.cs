using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigBullet : Upgrades
{
    public GameObject projectile;
    public float projectileSpeed;
    public Sprite CanonWeapon;

    public override void BuyFirst()
    {
        player.GetComponent<SpriteRenderer>().sprite = CanonWeapon;
        Weapons weapons = player.GetComponent<Weapons>();
        weapons.fireMode = "FireBigSlowBullet";
        //weapons.projectileSpeed = projectileSpeed / 4;
        //GameObject playerprojectile = Instantiate(projectile, transform.position, transform.rotation);
        //playerprojectile.GetComponent<PlayerProjectile>();
        //playerprojectile.transform.localScale *= 5.2f;

    }

}


