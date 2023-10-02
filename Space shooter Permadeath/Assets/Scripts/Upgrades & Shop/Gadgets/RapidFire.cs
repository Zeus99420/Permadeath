using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RapidFire : Gadgets
{
    public float baseFireRateMultiplier;
    public float fireRateMultiplierIncrease;
    public float baseMaxEnergy;
    public float maxEnergyIncrease;
    public override void BuyFirst() 
    {
        Weapons weapons = player.GetComponent<Weapons>();
        int index = weapons.methodSequence.IndexOf("RapidFireCheck");
        weapons.enabledSequence[index] = true;
        index = weapons.methodSequence.IndexOf("StandardFireCheck");
        weapons.enabledSequence[index] = false;
        weapons.rapidFireMultiplier = baseFireRateMultiplier;
        weapons.rapidFireEnergyMax = baseMaxEnergy;
        weapons.rapidFireEnergy = weapons.rapidFireEnergyMax;
    }

    public override void BuyAnother()
    {
        Weapons weapons = player.GetComponent<Weapons>();
        weapons.rapidFireMultiplier += fireRateMultiplierIncrease;
        weapons.rapidFireEnergyMax += maxEnergyIncrease;

    }






}
