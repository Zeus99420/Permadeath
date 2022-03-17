using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Upgrades
{
    public int baseBulletCount;
    public int bulletCountIncrease;
    public int maxSpread;
    public float damageMultiplier;
    public override void BuyFirst() 
    {
        Weapons weapons = player.GetComponent<Weapons>();
        weapons.fireMode = "ShotgunFire";
        weapons.spreadBulletCount = baseBulletCount;
        weapons.maxSpread = maxSpread;
        weapons.spreadDamageMultiplier = damageMultiplier;
    }

    public override void BuyAnother()
    {
        Weapons weapons = player.GetComponent<Weapons>();
        weapons.spreadBulletCount += bulletCountIncrease;

    }






}
