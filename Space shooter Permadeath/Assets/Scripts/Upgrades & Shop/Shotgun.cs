using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Upgrades
{
    public int baseBulletCount;
    public int bulletCountIncrease;
    public float baseSpread;
    public float spreadIncrease;
    public float damageMultiplier;
    public override void BuyFirst() 
    {
        Weapons weapons = player.GetComponent<Weapons>();
        //weapons.fireMode = "ShotgunFire";
        int index = weapons.methodSequence.IndexOf("ShotgunFire");
        weapons.enabledSequence[index] = true;
        index = weapons.methodSequence.IndexOf("StandardFire");
        weapons.enabledSequence[index] = false;
        weapons.spreadBulletCount = baseBulletCount;
        weapons.spread = baseSpread;
        weapons.spreadDamageMultiplier = damageMultiplier;
    }

    public override void BuyAnother()
    {
        Weapons weapons = player.GetComponent<Weapons>();
        weapons.spreadBulletCount += bulletCountIncrease;
        weapons.spread += spreadIncrease;

    }






}
