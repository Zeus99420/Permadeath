using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrdinanceSalvo : Upgrades
{
    public float maxChargesIncrease;
    public float rateOfFireIncrease;
    public override void Buy()
    {
        SecondaryWeapons ordinance = player.GetComponent<SecondaryWeapons>();
        ordinance.maxChargeMultiplier += maxChargesIncrease;
        ordinance.rateOfFireMultiplier += rateOfFireIncrease;
        ordinance.InitializeAmmoIndicator();
    }


    public override string GetDescription()
    {
        SecondaryWeapons ordinance = player.GetComponent<SecondaryWeapons>();
        float currentMaxCharges = ordinance.maxCharges * ordinance.maxChargeMultiplier;
        float newMaxCharges = ordinance.maxCharges * (ordinance.maxChargeMultiplier + maxChargesIncrease);
        float currentRateOfFire = ordinance.rateOfFire * ordinance.rateOfFireMultiplier;
        float newRateOfFire = ordinance.rateOfFire * (ordinance.rateOfFireMultiplier + rateOfFireIncrease);
        return ("Your ordinance can store more charges and can also be fired faster." +
            "\n\nMax Charges: " + currentMaxCharges + " -> " + newMaxCharges +
            "\nRate of Fire: " + currentRateOfFire + " -> " + newRateOfFire
            );
    }
}
