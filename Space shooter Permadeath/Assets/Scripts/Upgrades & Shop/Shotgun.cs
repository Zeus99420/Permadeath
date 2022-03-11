using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Upgrades
{
    public int bulletCountIncrease;
    public int maxSpread;
    public float damageMultiplier;
    public override void Buy() 
    {
        Weapons weapons = player.GetComponent<Weapons>();
        weapons.Fire = weapons.ShotgunFire;
        weapons.spreadBulletCount += bulletCountIncrease;
        weapons.maxSpread = maxSpread;
        weapons.spreadDamageMultiplier = damageMultiplier;
        Debug.Log(weapons.spreadBulletCount);
    }




   
   
}
