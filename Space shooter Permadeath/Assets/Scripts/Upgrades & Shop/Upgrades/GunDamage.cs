using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunDamage : Upgrades
{
    public int increaseAmount;
    public override void Buy()
    {
        Weapons weapons = player.GetComponent<Weapons>();
        weapons.baseDamage += (increaseAmount * weapons.damageMultiplier);
    }


    public override string GetDescription()
    {
        float currentDamage = player.GetComponent<Weapons>().baseDamage;
        float increase = (increaseAmount * player.GetComponent<Weapons>().damageMultiplier);
        return ("Your gun deals more damage." +
            "\n\nGun Damage : " + currentDamage + " -> " + (currentDamage + increase)
            );
    }
}
