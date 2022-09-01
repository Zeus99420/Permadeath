using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigBullet : Upgrades
{
    public GameObject projectile;
    public float projectileSpeed;
    public Sprite CanonWeapon;
    public float damageMultiplier;
    public float rateOfFireMultiplier;
    public float projectileSize;
    public Sprite projectileSprite;

    public override void BuyFirst()
    {
        Weapons weapons = player.GetComponent<Weapons>();
        weapons.weapon.GetComponent<SpriteRenderer>().sprite = CanonWeapon;
        //PlayerProjectile playerprojectile = weapons.projectile.GetComponent<PlayerProjectile>();
        //playerprojectile.GetComponent<SpriteRenderer>().sprite = Circle;
        weapons.bigBulletSprite = projectileSprite;
        weapons.fireMode = "FireBigSlowBullet";
        weapons.rateOfFire *= rateOfFireMultiplier;
        weapons.rateOfFireMultiplier *= rateOfFireMultiplier;
        weapons.projectileSize *= projectileSize;
        weapons.canonDamageMultiplier = damageMultiplier;
        weapons.piercing = true;
    }

    public override string GetDescription()
    {
        return ("Your projectiles are bigger, slower and deal more damage." +
            " If an enemy is killed the projectile keeps flying with any leftover damage " +
            "\n\nDamage: x" + damageMultiplier +
            "\nRate of Fire: x" + rateOfFireMultiplier
            );
    }

}


