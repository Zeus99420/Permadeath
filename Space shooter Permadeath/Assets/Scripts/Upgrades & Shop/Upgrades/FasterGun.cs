using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FasterGun : Upgrades
{
    public float increaseAmount;
    public Sprite BiggerGun;
    public override void Buy() 
    {
        Weapons weapons = player.GetComponent<Weapons>();
        weapons.weapon.GetComponent<SpriteRenderer>().sprite = BiggerGun;
        weapons.rateOfFire += increaseAmount * weapons.rateOfFireMultiplier;
    }


    public override string GetDescription()
    {
        float currentRateofFire = player.GetComponent<Weapons>().rateOfFire;
        float increase = increaseAmount * player.GetComponent<Weapons>().rateOfFireMultiplier;
        return ("Increases the rate of fire of your gun." +
            "\n\nRate of Fire : " + currentRateofFire + " -> " + (currentRateofFire + increase + " shots per second")
            );
    }
}
