using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigBullet : Gadgets
{
    public GameObject projectile;
    public float projectileSpeed;
    public Sprite CanonWeapon;
    public float damageMultiplier;
    public float rateOfFireMultiplier;
    public float projectileSpeedMultiplier;
    public float projectileSize;
    public Sprite projectileSprite;

    public override void BuyFirst()
    {
        Weapons weapons = player.GetComponent<Weapons>();
        weapons.weapon.GetComponent<SpriteRenderer>().sprite = CanonWeapon;
        weapons.projectileSprite = projectileSprite;
        weapons.rateOfFire *= rateOfFireMultiplier;
        weapons.rateOfFireMultiplier *= rateOfFireMultiplier;
        weapons.projectileSize *= projectileSize;
        weapons.damageMultiplier *= damageMultiplier;
        weapons.baseDamage *= damageMultiplier;
        weapons.projectileSpeed *= projectileSpeedMultiplier;
        weapons.projectileSpeedMultiplier *= projectileSpeedMultiplier;
        weapons.piercing = true;
    }

    public override string GetDescription()
    {
        return ("Your gun fires larger, slower projectiles that deal more damage." +
            " If an enemy is killed the projectile keeps flying with any leftover damage " +
            "\n\nDamage: x" + damageMultiplier +
            "\nRate of Fire: x" + rateOfFireMultiplier +
            "\nProjectile Velocity: x" + projectileSpeed
            );
    }

}


