using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RapidFire : Upgrades
{
    public float fireRateMultiplierIncrease;
    public float maxEnergyIncrease;
    public override void Buy() 
    {
        Weapons weapons = player.GetComponent<Weapons>();
        weapons.FireCheck = weapons.RapidFireCheck;
        weapons.rapidFireMultiplier += fireRateMultiplierIncrease;
        weapons.rapidFireEnergyMax += maxEnergyIncrease;
    }




   
   
}
