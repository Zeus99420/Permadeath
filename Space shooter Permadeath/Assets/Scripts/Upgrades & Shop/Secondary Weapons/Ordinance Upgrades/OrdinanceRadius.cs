using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrdinanceRadius : Upgrades
{
    public float increaseAmount;
    public override void Buy()
    {
        SecondaryWeapons ordinance = player.GetComponent<SecondaryWeapons>();
        ordinance.radiusMultiplier += increaseAmount;
    }


    public override string GetDescription()
    {
        float currentAmount = player.GetComponent<SecondaryWeapons>().radiusMultiplier;
        return ("Increases the explosive radius of all your ordinance." +
            "\n\nOrdinance Damage: " + currentAmount * 100 + "% -> " + (currentAmount + increaseAmount) * 100 + "%"
            );
    }
}
