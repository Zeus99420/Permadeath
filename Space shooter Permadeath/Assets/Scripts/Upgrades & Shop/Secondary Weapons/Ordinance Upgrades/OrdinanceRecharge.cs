using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrdinanceRecharge : Upgrades
{
    public float increaseAmount;
    public override void Buy()
    {
        SecondaryWeapons ordinance = player.GetComponent<SecondaryWeapons>();
        ordinance.rechargeRate += increaseAmount;
    }


    public override string GetDescription()
    {
        float currentAmount = player.GetComponent<SecondaryWeapons>().rechargeRate;
        return ("Your ordinance recharges faster." +
            "\n\nRecharge Rate: " + currentAmount * 100 + "% -> " + (currentAmount + increaseAmount) * 100 + "%"
            );
    }
}
