using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Gadgets
{
    public int baseBulletCount;
    public int bulletCountIncrease;
    public float baseSpread;
    public float spreadIncrease;
    public float damageMultiplier;
    public float rateOfFireMultiplier;
    public float projectileSize;
    
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
        weapons.baseDamage = (weapons.baseDamage*damageMultiplier);
        weapons.damageMultiplier *= damageMultiplier;
        weapons.rateOfFire *= rateOfFireMultiplier;
        weapons.rateOfFireMultiplier *= rateOfFireMultiplier;
        weapons.projectileSize *= projectileSize;

        upgradeName = "Upgrade Shotgun";
    }

    public override void BuyAnother()
    {
        Weapons weapons = player.GetComponent<Weapons>();
        weapons.spreadBulletCount += bulletCountIncrease;
        weapons.spread += spreadIncrease;

    }


    public override string GetDescription()
    {
        if (firstTimeBuying) return
                    ("You fire multiple smaller projectiles in an arc." +
                    "\n\nnumber of projectiles: " + baseBulletCount +
                    "\ndamage per projectile: x" + damageMultiplier +
                    "\nrate of fire: x" + rateOfFireMultiplier
                    );

        else
        {
            int currentBulletCount = player.GetComponent<Weapons>().spreadBulletCount;
            return ("Increase the number of projectiles fired." +
        "\n\nnumber of projectiles: " + currentBulletCount + " -> " + (currentBulletCount+bulletCountIncrease) );
        }


    }






}
