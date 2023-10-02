using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrdinanceDamage : Upgrades
{
    public float increaseAmount;
    public override void Buy()
    {
        SecondaryWeapons ordinance = player.GetComponent<SecondaryWeapons>();
        ordinance.damageMultiplier += increaseAmount;
    }


    public override string GetDescription()
    {
        float currentAmount = player.GetComponent<SecondaryWeapons>().damageMultiplier;
        return ("Increases all damage done by your ordinance." +
            "\n\nOrdinance Damage: " + currentAmount * 100 + "% -> " + (currentAmount + increaseAmount)*100 + "%"
            );
    }
}
