using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigBullet : Upgrades
{
    public GameObject projectile;
    public float projectileSpeed;
    public Sprite CanonWeapon;
    public float rateOfFireMultiplier;
    public float projectileSize;
    public Sprite Circle;

    public override void BuyFirst()
    {
        Weapons weapons = player.GetComponent<Weapons>();
        weapons.weapon.GetComponent<SpriteRenderer>().sprite = CanonWeapon;
        PlayerProjectile playerprojectile = player.GetComponent<PlayerProjectile>();
        playerprojectile.GetComponent<SpriteRenderer>().sprite = Circle;
        weapons.fireMode = "FireBigSlowBullet";
        weapons.rateOfFire *= rateOfFireMultiplier;
        weapons.rateOfFireMultiplier *= rateOfFireMultiplier;
        weapons.projectileSize *= projectileSize;


    }

}


