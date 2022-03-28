using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigBullet : Upgrades
{
    public GameObject projectile;
    public float projectileSpeed;
    public Sprite CanonWeapon;
    public float rateOfFireMultiplier;

    public override void BuyFirst()
    {
        Weapons weapons = player.GetComponent<Weapons>();
        weapons.weapon.GetComponent<SpriteRenderer>().sprite = CanonWeapon;
        weapons.fireMode = "FireBigSlowBullet";
        weapons.rateOfFire *= rateOfFireMultiplier;
        weapons.rateOfFireMultiplier *= rateOfFireMultiplier;
        //weapons.projectileSpeed = projectileSpeed / 4;
        //GameObject playerprojectile = Instantiate(projectile, transform.position, transform.rotation);
        //playerprojectile.GetComponent<PlayerProjectile>();
        //playerprojectile.transform.localScale *= 5.2f;

    }

}


