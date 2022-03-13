using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RapidFire : Upgrades
{
    public float baseFireRateMultiplier;
    public float fireRateMultiplierIncrease;
    public float baseMaxEnergy;
    public float maxEnergyIncrease;
    public override void BuyFirst() 
    {
        Weapons weapons = player.GetComponent<Weapons>();
        weapons.FireCheck = weapons.RapidFireCheck;
        weapons.rapidFireMultiplier = baseFireRateMultiplier;
        weapons.rapidFireEnergyMax = baseMaxEnergy;
    }

    public override void BuyAnother()
    {
        Weapons weapons = player.GetComponent<Weapons>();
        weapons.rapidFireMultiplier += fireRateMultiplierIncrease;
        weapons.rapidFireEnergyMax += maxEnergyIncrease;

    }






}
