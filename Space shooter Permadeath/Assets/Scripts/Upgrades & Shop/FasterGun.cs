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

   
   
}
